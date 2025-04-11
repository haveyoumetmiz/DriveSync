# DriveSync

## 🚀 Overview
**DriveSync** redefines racing game controls by using **hand gestures** via **real-time computer vision** instead of traditional input devices. Developed in Unity, and powered by **OpenCV**, **Mediapipe**, and **Python**, this project lets you steer, accelerate, and brake using nothing but your hands.

---

## 🎮 Demo Video

[![DriveSync Demo](Assets/demo-thumbnail.png)](Assets/demo.mp4)  
🔘 _Click the image to view/download the gameplay demo video._

---

## ✨ Features
- 🖐️ **Hand Gesture Controls** via Python + Mediapipe
- 🕹️ **Real-Time Game Steering** in Unity
- 🔁 **Live Communication** through UDP socket
- 🎮 **Immersive, Controller-Free Gameplay**
- 🧠 Great project to showcase CV + Unity skills

---

## 🔧 Setup & Execution

1. **Install Python Dependencies**  
   Ensure you have Python 3.x and run the following:
   ```bash
   pip install opencv-python mediapipe pyinput
Enable UDP Communication
This system uses UDP sockets to send data from Python (gesture detection) to Unity.

Run Gesture Recognition Script

bash
Copy code
python gesture_recognition.py
This opens the webcam.

Detects hand position and gestures (left/right/palm/fist).

Sends control data via UDP to Unity.

Open Unity Project

Open the project in Unity Hub.

Locate and open the handgesture.cs script inside Unity.

This script listens for UDP packets from Python and triggers game inputs accordingly.

Play the Game

Press the Play button in Unity Editor.

Start controlling the car with your hands! ✋➡️🏎️

🧠 How It Works
Architecture Flow:
text
Copy code
Webcam → Python (OpenCV + Mediapipe) → gesture_recognition.py
         ↓
       UDP Socket (Python → Unity)
         ↓
Unity → handgesture.cs → Car Control
gesture_recognition.py detects gestures and sends signals.

handgesture.cs in Unity listens to the UDP port and reacts accordingly.

Result: You control the car using your hand movements.

📦 Tech Stack
Unity (C#) – Game logic and rendering

Python (OpenCV + Mediapipe) – Gesture detection

UDP Socket – Real-time data transmission

pyinput / pydirectinput – Keyboard emulation (optional use)

🕹️ Gesture Mappings
Hand Gesture	Action
Hand Right	Turn Right
Hand Left	Turn Left
Open Palm	Accelerate
Closed Fist	Brake
🚧 Future Enhancements
🎨 Gesture customization via UI

🌐 Online multiplayer support

🔧 Smarter calibration and gesture filtering

🧠 ML-based gesture recognition

🕶️ AR headset integration

👥 Contributing
Pull requests are welcome! If you find bugs or want to add features, feel free to fork and improve the project.

📄 License
This project is licensed under the MIT License.

📬 Contact
📧 Email: mizh48.ansar@gmail.com

💼 LinkedIn: Mizhab Ansar

🧑‍💻 GitHub: haveyoumetmiz

📷 Instagram: @haveyoumetmiz

