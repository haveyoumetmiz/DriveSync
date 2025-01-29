using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    #region Settings
    [Header("Movement Settings")]
    [SerializeField] private float motorForce = 2000f;
    [SerializeField] private float brakeForce = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;
    
    [Header("Boost Settings")]
    [SerializeField] private float boostForce = 3000f;
    [SerializeField] private float boostDuration = 2f;
    
    [Header("Reposition Settings")]
    [SerializeField] private float positionRecordInterval = 0.1f;
    [SerializeField] private float historyDuration = 10f;
    [SerializeField] private float flipThreshold = 0.6f; // Threshold for considering the car flipped (0-1)
    [SerializeField] private float autoRecoverDelay = 3f; // Time before auto-recovery
    [SerializeField] private float stabilizationForce = 50f; // Force to stabilize the car
    [SerializeField] private float rightingTorque = 500f; // Torque to right the car
    #endregion

    #region Wheel References
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;
    #endregion

    #region Private Variables
    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBrakeForce;
    private bool isBraking;
    
    private Rigidbody carRigidbody; // Renamed to fix warning
    private bool isBoosting;
    private float boostEndTime;
    private float lastUprightTime;
    
    private List<Vector3> positionHistory = new List<Vector3>();
    private List<Quaternion> rotationHistory = new List<Quaternion>();
    private int maxHistoryEntries;
    private const KeyCode BOOST_KEY = KeyCode.LeftShift;
    private const KeyCode REPOSITION_KEY = KeyCode.R;
    #endregion

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        maxHistoryEntries = Mathf.CeilToInt(historyDuration / positionRecordInterval);
        StartCoroutine(RecordPosition());
        
        // Lower center of mass to prevent flipping
        carRigidbody.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    private void Update()
    {
        HandleInput();
        CheckAutoRecover();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        UpdateWheels();
        ApplyStabilization();
    }

    private void HandleInput()
    {
        // Movement inputs
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);

        // Special actions
        if (Input.GetKeyDown(REPOSITION_KEY)) RepositionCar();
        if (Input.GetKeyDown(BOOST_KEY)) StartBoost();
    }

    private void HandleMovement()
    {
        HandleSteering();
        HandleMotor();
        HandleBrakes();
        HandleBoostState();
    }
    private void HandleMotor()
    {
        float currentMotorForce = isBoosting ? motorForce + boostForce : motorForce;

        if (Mathf.Approximately(verticalInput, 0)) // If no input, gradually stop the car
        {
            frontLeftWheelCollider.motorTorque = 0f;
            frontRightWheelCollider.motorTorque = 0f;
            carRigidbody.velocity = Vector3.Lerp(carRigidbody.velocity, Vector3.zero, Time.fixedDeltaTime * 2f); // Smooth stop
        }
        else
        {
            frontLeftWheelCollider.motorTorque = verticalInput * currentMotorForce;
            frontRightWheelCollider.motorTorque = verticalInput * currentMotorForce;
        }
    }

    private void HandleBrakes()
    {
        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBrakeForce();
    }

    private void ApplyBrakeForce()
    {
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheel(WheelCollider collider, Transform transform)
    {
        collider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        transform.SetPositionAndRotation(position, rotation);
    }

    private void StartBoost()
    {
        if (isBoosting) return;
        isBoosting = true;
        boostEndTime = Time.time + boostDuration;
    }

    private void HandleBoostState()
    {
        if (isBoosting && Time.time > boostEndTime)
        {
            isBoosting = false;
        }
    }

    private void RepositionCar()
    {
        if (positionHistory.Count == 0) return;

        int targetIndex = Mathf.Max(0, positionHistory.Count - maxHistoryEntries);
        carRigidbody.velocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(positionHistory[targetIndex], rotationHistory[targetIndex]);
    }

    private IEnumerator RecordPosition()
    {
        while (true)
        {
            positionHistory.Add(transform.position);
            rotationHistory.Add(transform.rotation);

            // Maintain history size
            while (positionHistory.Count > maxHistoryEntries)
            {
                positionHistory.RemoveAt(0);
                rotationHistory.RemoveAt(0);
            }

            yield return new WaitForSeconds(positionRecordInterval);
        }
    }

    private void ApplyStabilization()
    {
        if (IsUpright())
        {
            // Apply extra downward force when upright for better traction
            carRigidbody.AddForce(-transform.up * stabilizationForce, ForceMode.Acceleration);
        }
        else
        {
            // Try to right the car when not upright
            RightCar();
        }
    }

    private bool IsUpright()
    {
        // Check if car is flipped using dot product with world up
        return Vector3.Dot(transform.up, Vector3.up) > flipThreshold;
    }

    private void RightCar()
    {
        // Only allow righting if car is not moving much
        if (carRigidbody.velocity.magnitude < 2f)
        {
            // Calculate target rotation
            Quaternion targetRot = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
            
            // Smoothly rotate towards target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.fixedDeltaTime * 2f);
            
            // Add stabilizing torque
            carRigidbody.AddTorque(transform.forward * rightingTorque);
        }
    }

    private void CheckAutoRecover()
    {
        if (!IsUpright())
        {
            if (Time.time - lastUprightTime > autoRecoverDelay)
            {
                RepositionCar();
            }
        }
        else
        {
            lastUprightTime = Time.time;
        }
    }
    public float GetMaxSpeed()
    {
        return 50f; // Adjust based on your car's top speed
    }

}