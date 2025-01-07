using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public Rigidbody carRigidbody;  // Reference to the car's Rigidbody
    public Text speedText;          // Reference to the UI Text component

    void Update()
    {
        // Calculate speed in km/h (Rigidbody velocity is in meters per second)
        float speed = carRigidbody.velocity.magnitude * 3.6f;

        // Update the speed text
        speedText.text = speed.ToString("0") + " km/h";
    }
}
