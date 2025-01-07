using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class ButtonController : MonoBehaviour
{
    // This function is called when the Start button is clicked
    public void OnStartButtonClicked()
    {
        // Load the "MainScene" (replace with your actual main scene name)
        SceneManager.LoadScene("Scenes/SampleScene");
    }

    // This function is called when the Menu button is clicked
    public void OnMenuButtonClicked()
    {
        // Load the "StartMenu" (replace with your actual start menu scene name)
        SceneManager.LoadScene("Scenes/Start");
    }

    // This function is called when the Settings button is clicked
    public void OnSettingsButtonClicked()
    {
        // Load the "Settings" (replace with your actual settings scene name)
        SceneManager.LoadScene("Scenes/settings");
    }
}
