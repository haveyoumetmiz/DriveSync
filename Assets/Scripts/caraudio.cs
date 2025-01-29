using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarAudio : MonoBehaviour
{
    #region Settings
    [Header("Engine Sounds")]
    [SerializeField] private AudioClip engineIdleClip;
    [SerializeField] private AudioClip engineRunningClip;
    [SerializeField] private float pitchMin = 0.8f;
    [SerializeField] private float pitchMax = 2.0f;
    [SerializeField] private float volumeMin = 0.1f;
    [SerializeField] private float volumeMax = 0.5f;

    [Header("Skid Sounds")]
    [SerializeField] private AudioClip skidClip;
    [SerializeField] private float skidVolume = 0.5f;
    [SerializeField] private float skidThreshold = 0.5f; // Speed threshold for skid sounds

    [Header("Collision Sounds")]
    [SerializeField] private AudioClip collisionClip;
    [SerializeField] private float collisionVolume = 0.5f;
    [SerializeField] private float collisionThreshold = 2.0f; // Minimum collision force to play sound
    #endregion

    #region Private Variables
    private AudioSource engineAudioSource;
    private AudioSource skidAudioSource;
    private AudioSource collisionAudioSource;
    private CarController carController;
    private Rigidbody carRigidbody;  // Added Rigidbody
    private WheelCollider[] wheelColliders;
    private bool isSkidding;
    #endregion

    private void Awake()
    {
        carController = GetComponent<CarController>();
        carRigidbody = GetComponent<Rigidbody>(); // Get the car's Rigidbody
        wheelColliders = GetComponentsInChildren<WheelCollider>();

        // Set up audio sources
        engineAudioSource = gameObject.AddComponent<AudioSource>();
        engineAudioSource.loop = true;
        engineAudioSource.clip = engineIdleClip;
        engineAudioSource.Play();

        skidAudioSource = gameObject.AddComponent<AudioSource>();
        skidAudioSource.loop = true;
        skidAudioSource.clip = skidClip;
        skidAudioSource.volume = skidVolume;

        collisionAudioSource = gameObject.AddComponent<AudioSource>();
        collisionAudioSource.clip = collisionClip;
        collisionAudioSource.volume = collisionVolume;
    }

    private void Update()
    {
        HandleEngineSound();
        HandleSkidSound();
    }

    private void HandleEngineSound()
    {
        if (carController == null) return;

        // Calculate engine pitch and volume based on speed
        float speedRatio = GetSpeedRatio(); // Using the helper method
        engineAudioSource.pitch = Mathf.Lerp(pitchMin, pitchMax, speedRatio);
        engineAudioSource.volume = Mathf.Lerp(volumeMin, volumeMax, speedRatio);

        // Switch between idle and running clips
        if (speedRatio > 0.1f && engineAudioSource.clip != engineRunningClip)
        {
            engineAudioSource.clip = engineRunningClip;
            engineAudioSource.Play();
        }
        else if (speedRatio <= 0.1f && engineAudioSource.clip != engineIdleClip)
        {
            engineAudioSource.clip = engineIdleClip;
            engineAudioSource.Play();
        }
    }

    private void HandleSkidSound()
    {
        if (wheelColliders == null || wheelColliders.Length == 0) return;

        isSkidding = false;
        foreach (var wheel in wheelColliders)
        {
            WheelHit hit;
            if (wheel.GetGroundHit(out hit))
            {
                if (Mathf.Abs(hit.forwardSlip) > skidThreshold || Mathf.Abs(hit.sidewaysSlip) > skidThreshold)
                {
                    isSkidding = true;
                    break;
                }
            }
        }

        if (isSkidding && !skidAudioSource.isPlaying)
        {
            skidAudioSource.Play();
        }
        else if (!isSkidding && skidAudioSource.isPlaying)
        {
            skidAudioSource.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > collisionThreshold)
        {
            collisionAudioSource.PlayOneShot(collisionClip);
        }
    }

    // Helper method to calculate speed ratio (0-1)
    private float GetSpeedRatio()
    {
        if (carRigidbody == null) return 0f;
        return Mathf.Clamp01(GetCurrentSpeed() / GetMaxSpeed());
    }

    public float GetCurrentSpeed()
    {
        return carRigidbody.velocity.magnitude; // Uses Rigidbody velocity to determine speed
    }

    public float GetMaxSpeed()
    {
        return carController != null ? carController.GetMaxSpeed() : 20f; // Default max speed if CarController is null
    }
}
