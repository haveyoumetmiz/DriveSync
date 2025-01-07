import cv2
import numpy as np

# Initialize webcam
cap = cv2.VideoCapture(0)

# Variables to store the locked circle's information
locked_circle = None
prev_x = None
direction_threshold = 10  # Sensitivity for detecting left/right movement

# Function to reset locked circle
def reset_locked_circle():
    global locked_circle
    locked_circle = None

while True:
    # Capture frame from webcam
    ret, frame = cap.read()
    
    if not ret:
        print("Failed to grab frame")
        break
    
    # Convert frame to grayscale
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    
    # Apply Gaussian blur to reduce noise
    blurred = cv2.GaussianBlur(gray, (9, 9), 2)
    
    if locked_circle is None:
        # Detect circle (plate) using Hough Circles
        circles = cv2.HoughCircles(blurred, cv2.HOUGH_GRADIENT, dp=1.2, minDist=50,
                                   param1=50, param2=30, minRadius=15, maxRadius=300)
        if circles is not None:
            circles = np.round(circles[0, :]).astype("int")
            # Lock the largest circle
            locked_circle = max(circles, key=lambda x: x[2])  # Get largest circle
    
    if locked_circle is not None:
        x, y, r = locked_circle
        
        # Check if the circle is still within the frame
        if x - r < 0 or x + r >= frame.shape[1] or y - r < 0 or y + r >= frame.shape[0]:
            # If the circle is out of bounds, reset it
            reset_locked_circle()

        # Detect movement direction (left or right)
        if prev_x is not None:
            if x < prev_x - direction_threshold:  # Moving left
                direction = "left"
            elif x > prev_x + direction_threshold:  # Moving right
                direction = "right"
            else:
                direction = "none"
            
            # Write direction to file
            with open("direction.txt", "w") as f:
                f.write(direction)
        
        prev_x = x
    
    # Display frame with the detected circle
    if locked_circle is not None:
        cv2.circle(frame, (x, y), r, (0, 255, 0), 4)  # Draw the circle
    
    cv2.imshow("Tracking", frame)
    
    # Exit on 'q'
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Release webcam and close windows
cap.release()
cv2.destroyAllWindows()
