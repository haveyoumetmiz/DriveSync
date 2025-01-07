using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BlackRectangleDetection : MonoBehaviour
{
    public RawImage displayImage; // UI RawImage to display the webcam feed
    public float minAspectRatio = 1.5f; // Minimum aspect ratio for a rectangle (width/height)
    public float maxAspectRatio = 2.5f; // Maximum aspect ratio for a rectangle (width/height)
    public float colorThreshold = 0.2f; // Color matching threshold for detecting black

    private WebCamTexture webcamTexture;
    private Texture2D outputTexture;

    void Start()
    {
        // Start the webcam
        webcamTexture = new WebCamTexture();
        webcamTexture.Play();

        // Set up the display texture
        outputTexture = new Texture2D(webcamTexture.width, webcamTexture.height);
        displayImage.texture = outputTexture;
    }

    void Update()
    {
        if (webcamTexture.isPlaying)
        {
            // Update the texture from the webcam
            Color32[] pixels = webcamTexture.GetPixels32();
            outputTexture.SetPixels32(pixels);
            outputTexture.Apply();

            DetectBlackRectangle(pixels, webcamTexture.width, webcamTexture.height);
        }
    }

    void DetectBlackRectangle(Color32[] pixels, int width, int height)
    {
        List<Rect> detectedRectangles = new List<Rect>();

        // Loop through the pixels and find potential black areas (rectangles)
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (IsBlackPixel(pixels[y * width + x]))
                {
                    // Start checking for a rectangle from this pixel
                    Rect rect = FindRectangle(pixels, x, y, width, height);
                    if (rect.width > 20 && rect.height > 20) // Ignore small rectangles
                    {
                        // Only add the rectangle if it has a reasonable aspect ratio (for phone shape)
                        float aspectRatio = rect.width / rect.height;
                        if (aspectRatio >= minAspectRatio && aspectRatio <= maxAspectRatio)
                        {
                            detectedRectangles.Add(rect);
                        }
                    }
                }
            }
        }

        // Draw the detected rectangles
        foreach (Rect rect in detectedRectangles)
        {
            DrawRectangle(rect);
        }

        outputTexture.Apply();
    }

    bool IsBlackPixel(Color32 pixel)
    {
        // Check if the pixel is black or very dark
        float r = pixel.r / 255f;
        float g = pixel.g / 255f;
        float b = pixel.b / 255f;

        return r < colorThreshold && g < colorThreshold && b < colorThreshold;
    }

    Rect FindRectangle(Color32[] pixels, int startX, int startY, int width, int height)
    {
        int endX = startX;
        int endY = startY;

        // Extend the search for the right side of the rectangle
        while (endX < width && IsBlackPixel(pixels[startY * width + endX]))
        {
            endX++;
        }

        // Extend the search for the bottom side of the rectangle
        while (endY < height && IsBlackPixel(pixels[endY * width + startX]))
        {
            endY++;
        }

        return new Rect(startX, startY, endX - startX, endY - startY);
    }

    void DrawRectangle(Rect rect)
    {
        // Draw a rectangle around the detected black phone region
        int xStart = (int)rect.x;
        int yStart = (int)rect.y;
        int width = (int)rect.width;
        int height = (int)rect.height;

        // Draw the borders of the rectangle
        for (int x = xStart; x < xStart + width; x++)
        {
            for (int y = yStart; y < yStart + height; y++)
            {
                if (x == xStart || x == xStart + width - 1 || y == yStart || y == yStart + height - 1)
                {
                    outputTexture.SetPixel(x, y, Color.green); // Green rectangle border
                }
            }
        }
    }

    void OnDestroy()
    {
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
        }
    }
}
