using UnityEngine;

public class SimpleEngineSound : MonoBehaviour
{
    public AudioSource idleSource;
    public AudioSource accelerationSource;

    public Rigidbody carRigidbody;
    public float pitchMultiplier = 0.05f; // Increase for more noticeable pitch changes
    public float minPitch = 1.0f; // Ensure this is greater than 0
    public float maxPitch = 3.0f; // Higher max pitch for acceleration
    public float volumeMultiplier = 2.0f; // Control volume increase with speed
    public float maxIdleVolume = 100.0f; // Max volume for idle sound
    public float maxAccelerationVolume = 100.0f; // Max volume for acceleration sound

    void Start()
    {
        idleSource.Play(); // Start with idle sound
        accelerationSource.volume = 0f; // Start with the acceleration sound muted
    }

    void Update()
    {
        float speed = carRigidbody.velocity.magnitude; // Get the car's speed

        // Adjust pitch and volume based on speed for acceleration sound
        accelerationSource.pitch = Mathf.Clamp(minPitch + speed * pitchMultiplier, minPitch, maxPitch);
        accelerationSource.volume = Mathf.Clamp(speed * volumeMultiplier, 0f, maxAccelerationVolume);

        // Start acceleration sound when moving, stop when idle
        if (speed > 0.1f && !accelerationSource.isPlaying)
        {
            accelerationSource.Play();
            idleSource.volume = Mathf.Lerp(idleSource.volume, maxIdleVolume * 0.2f, Time.deltaTime * 5f); // Gradually lower idle sound volume during acceleration
        }
        else if (speed <= 0.1f && accelerationSource.isPlaying)
        {
            accelerationSource.Stop();
            idleSource.volume = Mathf.Lerp(idleSource.volume, maxIdleVolume, Time.deltaTime * 5f); // Gradually restore idle sound volume when stopped
        }

        // Adjust idle sound volume based on speed, with idle volume decreasing as speed increases
        idleSource.volume = Mathf.Clamp(maxIdleVolume - (speed * volumeMultiplier), 0.2f, maxIdleVolume);
    }
}
