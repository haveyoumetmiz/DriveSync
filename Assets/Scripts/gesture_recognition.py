import cv2
import mediapipe as mp
import socket  # Import socket for UDP

# Initialize Mediapipe Hands model
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(min_detection_confidence=0.7, min_tracking_confidence=0.7)
mp_draw = mp.solutions.drawing_utils

# Set up UDP socket
UDP_IP = "127.0.0.1"  # Localhost (or change to the IP of the Unity machine if running on a different machine)
UDP_PORT = 12345  # The port where Unity is listening for UDP messages
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# Start capturing video from the webcam
cap = cv2.VideoCapture(0)

while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        break

    # Flip the frame horizontally for a mirror effect
    frame = cv2.flip(frame, 1)

    # Convert the frame to RGB as Mediapipe requires RGB input
    rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

    # Process the frame and get hand landmarks
    results = hands.process(rgb_frame)

    # If hands are detected, draw landmarks and connections
    if results.multi_hand_landmarks:
        for hand_landmarks in results.multi_hand_landmarks:
            # Draw landmarks and connections
            mp_draw.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)

            # Extract coordinates of key landmarks (e.g., wrist and finger tips)
            wrist = hand_landmarks.landmark[0]  # Wrist landmark (index 0)
            height, width, _ = frame.shape
            wrist_x = int(wrist.x * width)

            # Gesture detection example (Open Palm vs. Closed Fist)
            thumb_tip = hand_landmarks.landmark[4]  # Thumb tip
            index_tip = hand_landmarks.landmark[8]  # Index tip
            distance = abs(thumb_tip.x - index_tip.x) + abs(thumb_tip.y - index_tip.y)

            # Detect Open Palm vs. Closed Fist
            if distance > 0.1:  # Threshold distance for open palm
                gesture = "Open Palm"
                cv2.putText(frame, "Open Palm", (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 0, 0), 2)
            else:
                gesture = "Closed Fist"
                cv2.putText(frame, "Closed Fist", (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 0, 0), 2)

            # Detect Left or Right side of the screen
            if wrist_x < width // 2:
                side = "Left"
                cv2.putText(frame, "Hand on Left Side", (50, 100), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)
            else:
                side = "Right"
                cv2.putText(frame, "Hand on Right Side", (50, 100), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 255), 2)

            # Send gesture and side information over UDP to Unity
            message = f"{gesture} - {side}"
            sock.sendto(message.encode(), (UDP_IP, UDP_PORT))

    # Display the resulting frame
    cv2.imshow('Hand Gesture Detection', frame)

    # Break the loop if the 'q' key is pressed
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Release resources
cap.release()
cv2.destroyAllWindows()
sock.close()
