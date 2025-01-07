using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBreakForce;
    private bool isBreaking;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;
    [SerializeField] private float boostForce = 3000f; // Force applied during boost
    [SerializeField] private float boostDuration = 2f; // Duration of the boost in seconds
    [SerializeField] private float positionRecordInterval = 0.1f; // Interval at which position is recorded

    private float originalMotorForce; // To store the original motor force

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private Rigidbody rb;
    private bool isBoosting;
    private float boostEndTime;

    // For storing positions over time
    private List<Vector3> positionHistory = new List<Vector3>();
    private List<Quaternion> rotationHistory = new List<Quaternion>();
    private float historyDuration = 10f; // Duration of history to keep (in seconds)

    private void Start() {
        rb = GetComponent<Rigidbody>();
        originalMotorForce = motorForce; // Store the original motor force
        StartCoroutine(RecordPosition());
    }

    private void FixedUpdate() {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        CheckReposition();
        HandleBoost();
    }

    private void GetInput() {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor() {
        // Apply boost if active
        float currentMotorForce = isBoosting ? motorForce + boostForce : motorForce;

        frontLeftWheelCollider.motorTorque = verticalInput * currentMotorForce;
        frontRightWheelCollider.motorTorque = verticalInput * currentMotorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking() {
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering() {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels() {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void CheckReposition() {
        if (Input.GetKeyDown(KeyCode.R)) {
            RepositionCarToHistory();
        }
    }

    private void RepositionCarToHistory() {
        if (positionHistory.Count > 0) {
            // Get the position and rotation from 10 seconds ago
            int index = Mathf.Max(0, positionHistory.Count - Mathf.RoundToInt(10f / positionRecordInterval));
            transform.position = positionHistory[index];
            transform.rotation = rotationHistory[index];
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void HandleBoost() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            StartBoost();
        }

        if (isBoosting && Time.time > boostEndTime) {
            EndBoost();
        }
    }

    private void StartBoost() {
        isBoosting = true;
        boostEndTime = Time.time + boostDuration;
    }

    private void EndBoost() {
        isBoosting = false;
        motorForce = originalMotorForce;
    }

    private IEnumerator RecordPosition() {
        while (true) {
            // Record the car's position and rotation
            positionHistory.Add(transform.position);
            rotationHistory.Add(transform.rotation);

            // Keep the history within the last 10 seconds
            if (positionHistory.Count > historyDuration / positionRecordInterval) {
                positionHistory.RemoveAt(0);
                rotationHistory.RemoveAt(0);
            }

            yield return new WaitForSeconds(positionRecordInterval);
        }
    }
}