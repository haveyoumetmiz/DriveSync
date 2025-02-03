using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class HandGestureControl : MonoBehaviour
{
    private UdpClient udpClient;
    private int port = 12345; // Port to match with Python code
    private Thread receiveThread;

    // Car control variables
    private float speed = 0f;
    private float turnSpeed = 0f;
    private float targetTurnSpeed = 0f;

    // Thread-safe variables to store gesture data
    private string receivedGesture = "";

    void Start()
    {
        // Initialize the UDP client and start a thread to receive messages
        udpClient = new UdpClient(port);
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        // Print message when the program starts and listens on the specified port
        Debug.Log($"Listening for UDP messages on port {port}...");
    }

    void Update()
    {
        // Check if there is any new gesture to handle
        if (!string.IsNullOrEmpty(receivedGesture))
        {
            HandleGesture(receivedGesture);
            receivedGesture = ""; // Reset after handling the gesture
        }

        // Move the car forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Smoothly rotate the car based on the gesture (right/left)
        float targetRotation = targetTurnSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * targetRotation);
    }

    // This method will run on a separate thread to receive UDP messages
    void ReceiveData()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

        while (true)
        {
            byte[] data = udpClient.Receive(ref endPoint); // Blocking call until data is received
            string message = Encoding.UTF8.GetString(data);
            receivedGesture = message; // Store the received gesture for processing in the main thread
        }
    }

    // Handle the received gesture message and control the car
    void HandleGesture(string message)
    {
        Debug.Log("Received Gesture: " + message);  // Debug log to see the received message

        if (message.Contains("Open Palm"))
        {
            speed = 10f; // Forward movement (adjust speed as needed)

            if (message.Contains("Right"))
            {
                targetTurnSpeed = 50f; // Turn Right
            }
            else if (message.Contains("Left"))
            {
                targetTurnSpeed = -50f; // Turn Left
            }
        }
        else if (message.Contains("Closed Fist"))
        {
            speed = 0f; // Stop car
            targetTurnSpeed = 0f; // No turning
        }
        else
        {
            speed = 0f; // No movement
            targetTurnSpeed = 0f; // No turning
        }
    }

    void OnApplicationQuit()
    {
        // Ensure the thread is properly stopped when the application quits
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }
    }
}
