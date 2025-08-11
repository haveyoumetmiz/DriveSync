import cv2
import mediapipe as mp
import socket
import os

# Initialize Mediapipe Hands
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(min_detection_confidence=0.7, min_tracking_confidence=0.7)
mp_draw = mp.solutions.drawing_utils

# UDP Setup
UDP_IP = "127.0.0.1"
UDP_PORT = 12345
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# Detect if running in GitHub Actions
running_in_ci = os.environ.get("GITHUB_ACTIONS") == "true"

if running_in_ci:
    print("CI detected — using sample video instead of webcam.")
    cap = cv2.VideoCapture("Assets/Scripts/sample_input.mp4")
else:
    print("Local run — using webcam.")
    cap = cv2.VideoCapture(0)

while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        print("No frame captured. Ending.")
        break

    frame = cv2.flip(frame, 1)
    height, width, _ = frame.shape
    rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    results = hands.process(rgb_frame)

    if results.multi_hand_landmarks:
        for hand_landmarks in results.multi_hand_landmarks:
            mp_draw.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)

            wrist = hand_landmarks.landmark[0]
            wrist_x = int(wrist.x * width)
            thumb_tip = hand_landmarks.landmark[4]
            index_tip = hand_landmarks.landmark[8]
            distance = abs(thumb_tip.x - index_tip.x) + abs(thumb_tip.y - index_tip.y)

            if distance > 0.1:
                gesture = "Open Palm"
                cv2.putText(frame, gesture, (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 0, 0), 2)
            else:
                gesture = "Closed Fist"
                cv2.putText(frame, gesture, (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 0, 0), 2)

            if wrist_x < width // 3:
                side = "Left"
            elif wrist_x > 2 * width // 3:
                side = "Right"
            else:
                side = "Center"

            cv2.putText(frame, f"Hand on {side} Side", (50, 100), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)

            message = f"{gesture} - {side}"
            sock.sendto(message.encode(), (UDP_IP, UDP_PORT))

    if not running_in_ci:
        cv2.imshow('Hand Gesture Detection', frame)
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

cap.release()
if not running_in_ci:
    cv2.destroyAllWindows()
sock.close()
