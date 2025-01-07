import cv2
import numpy as np

# Initialize webcam
cap = cv2.VideoCapture(0)

# Variables to store the locked circle's information
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
        # Initial circle detection
        circles = cv2.HoughCircles(blurred, 
                                   cv2.HOUGH_GRADIENT, 
                                   dp=1.2, 
                                   minDist=50, 
                                   param1=50, 
                                   param2=30, 
                                   minRadius=15, 
                                   maxRadius=300)
        if circles is not None:
            circles = np.round(circles[0, :]).astype("int")
            # Find the largest circle
            locked_circle = max(circles, key=lambda x: x[2])
    
    if locked_circle is not None:
        # Draw the locked circle
        x, y, r = locked_circle
        cv2.circle(frame, (x, y), r, (0, 255, 0), 4)
        cv2.rectangle(frame, (x - 5, y - 5), (x + 5, y + 5), (0, 128, 255), -1)
        
        # Optionally, re-check the circle position in subsequent frames
        # (This is a simple re-check; advanced tracking methods like CAMShift could be used for better tracking)
        # Create a region of interest (ROI) around the locked circle to re-detect in a smaller area
        x_min = max(x - r, 0)
        x_max = min(x + r, frame.shape[1])
        y_min = max(y - r, 0)
        y_max = min(y + r, frame.shape[0])
        
        roi = blurred[y_min:y_max, x_min:x_max]
        small_circles = cv2.HoughCircles(roi, 
                                         cv2.HOUGH_GRADIENT, 
                                         dp=1.2, 
                                         minDist=50, 
                                         param1=50, 
                                         param2=30, 
                                         minRadius=int(r*0.9), 
                                         maxRadius=int(r*1.1))
        if small_circles is not None:
            small_circles = np.round(small_circles[0, :]).astype("int")
            # Update locked circle's position if found
            new_circle = max(small_circles, key=lambda x: x[2])
            locked_circle = (x_min + new_circle[0], y_min + new_circle[1], new_circle[2])
    
    # Display the frame with the locked circle
    cv2.imshow('Locked Circle', frame)
    
    # Break loop if 'q' key is pressed
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Release the webcam and close windows
cap.release()
cv2.destroyAllWindows()
