# DriveSync

## Overview
This project introduces a unique control mechanism for a racing game developed in Unity. Instead of relying on traditional input methods like WASD keys or controllers, the game uses real-time hand tracking to enable intuitive controls. By leveraging OpenCV for camera input and Mediapipe for hand landmark detection, players can use hand gestures to steer their car, creating an immersive and innovative gaming experience that bridges the physical and digital worlds.

---

## Features
- **Hand Tracking with Mediapipe:** Detects and tracks hand landmarks using a webcam.
- **Real-Time Steering:** The hand’s position and gestures determine the car’s steering behavior in the game.
  - Hand on the right: Steer right.
  - Hand on the left: Steer left.
  - Open palm: Accelerate.
  - Closed fist: Brake.
- **Immersive Gameplay:** Merges physical hand movements with virtual controls for a unique experience.
- **Cross-Platform Support:** Built with Unity for easy deployment across multiple platforms.

---

## Technology Stack
- **Unity:** Game development platform for creating and managing the racing game.
- **OpenCV:** Library for accessing the webcam feed.
- **Mediapipe:** Framework for detecting hand landmarks and tracking gestures.
- **C#:** Programming language used for scripting within Unity.
- **Webcam:** Captures real-time video feed for detecting hand gestures.

---

## How It Works
1. **Setup and Calibration:**
   - The game initializes by capturing input from the webcam.
   - OpenCV processes the video feed, and Mediapipe detects hand landmarks in real time.
   
2. **Hand Landmark Detection:**
   - Mediapipe tracks the hand’s position, recognizing key landmarks and gestures.

3. **Input Mapping:**
   - Hand gestures are mapped to the car’s control behavior:
     - Hand on the right moves the car to the right.
     - Hand on the left moves the car to the left.
     - Open palm accelerates the car.
     - Closed fist applies the brake.

4. **Gameplay:**
   - Players can control the car using intuitive hand movements, offering an engaging and interactive gaming experience.

---

## Installation
### Prerequisites
- Unity (2021 or later recommended)
- OpenCV library installed
- Mediapipe package integrated with Unity
- A functional webcam connected to the system

### Steps
1. Clone the repository:
   ```bash
   git clone <https://github.com/haveyoumetmiz/car.git>
   ```
2. Open the project in Unity.
3. Install the required OpenCV and Mediapipe dependencies.
4. Connect a webcam to your system.
5. Play the game from the Unity Editor or build it for your target platform.

---

## Usage
1. Launch the game.
2. Use hand gestures to control the car:
   - Move hand to the right to turn right.
   - Move hand to the left to turn left.
   - Open palm to accelerate.
   - Closed fist to brake.
3. Enjoy the immersive racing experience!

---

## Future Enhancements
- **Gesture Customization:** Add support for different hand gestures for improved controls.
- **Multiplayer Mode:** Enable local or online multiplayer gaming.
- **Augmented Reality (AR) Integration:** Enhance visual interaction by overlaying AR elements.
- **Enhanced Calibration:** Improve the detection algorithm for faster and more accurate input recognition.

---

## Contributing
Contributions are welcome! If you’d like to contribute, please fork the repository and submit a pull request.

---

## License
This project is licensed under the [MIT License](LICENSE).

---

## Contact
For any inquiries or feedback, feel free to reach out:
- **Email:** mizh48.ansar@gmail.com
- **LinkedIn:** [Mizhab Ansar](https://www.linkedin.com/in/mizhabansar/)
- **GitHub:** [haveyoumetmiz](https://github.com/haveyoumetmiz)

---

Enjoy the game and experience the future of gaming controls!
