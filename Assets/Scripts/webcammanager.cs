using UnityEngine;
using UnityEngine.UI;  // Required for using UI elements like RawImage

public class WebcamManager : MonoBehaviour
{
    public RawImage display;  // This must be public to show up in the Inspector

    private WebCamTexture webcamTexture;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        
        if (devices.Length > 0)
        {
            webcamTexture = new WebCamTexture(devices[0].name);
            webcamTexture.Play();
            display.texture = webcamTexture;  // Assign the webcam feed to the RawImage
        }
        else
        {
            Debug.LogWarning("No webcam detected!");
        }
    }

    void OnDisable()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            webcamTexture.Stop();
        }
    }
}
