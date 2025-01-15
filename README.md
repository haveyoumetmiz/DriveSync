# Innovative Control System for Racing Game Using Real-Life Object Detection

## Overview
This project introduces a unique control mechanism for a racing game developed in Unity. Instead of relying on traditional input methods like WASD keys or controllers, the game uses real-life object detection to enable intuitive controls. By leveraging OpenCV for computer vision, players can use a circular object (e.g., a plate) to steer their car, creating an immersive and innovative gaming experience that bridges the physical and digital worlds.

---

## Features
- **Object Detection with OpenCV:** The system detects and tracks a circular object using a webcam.
- **Real-Time Steering:** The rotation of the object determines the car's steering direction in the game.
  - Clockwise rotation: Steer right.
  - Counterclockwise rotation: Steer left.
- **Immersive Gameplay:** Merges physical movements with virtual controls for a unique experience.
- **Cross-Platform Support:** Built with Unity for easy deployment across multiple platforms.

---

## Technology Stack
- **Unity:** Game development platform for creating and managing the racing game.
- **OpenCV:** Library for computer vision tasks, used for object detection and tracking.
- **C#:** Programming language used for scripting within Unity.
- **Webcam:** Captures real-time video feed for detecting the circular object.

---

## How It Works
1. **Setup and Calibration:**
   - The game initializes by capturing input from the webcam.
   - OpenCV processes the video feed to identify a circular object (e.g., a plate).
   
2. **Object Detection:**
   - OpenCV detects the position and orientation of the circular object in real-time.

3. **Input Mapping:**
   - The rotation of the object is mapped to the car's steering behavior:
     - Clockwise rotation moves the car to the right.
     - Counterclockwise rotation moves the car to the left.

4. **Gameplay:**
   - Players can steer the car in the racing game by simply rotating the circular object, offering an engaging and interactive gaming experience.

---

## Installation
### Prerequisites
- Unity (2021 or later recommended)
- OpenCV library installed
- A functional webcam connected to the system

### Steps
1. Clone the repository:
   ```bash
   git clone <https://github.com/haveyoumetmiz/car.git>
   ```
2. Open the project in Unity.
3. Install the required OpenCV dependencies.
4. Connect a webcam to your system.
5. Play the game from the Unity Editor or build it for your target platform.

---

## Usage
1. Launch the game.
2. Hold a circular object in front of the webcam.
3. Use the rotation of the object to control the car:
   - Rotate clockwise to turn right.
   - Rotate counterclockwise to turn left.
4. Enjoy the immersive racing experience!

---

## Future Enhancements
- **Shape Customization:** Add support for detecting different shapes (e.g., square or triangle) for controls.
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

