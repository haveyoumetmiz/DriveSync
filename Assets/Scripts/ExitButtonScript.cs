using UnityEngine;
using UnityEngine.UI;

public class ExitHandler : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void ExitApplication()
    {
        // Quit the application in a build
        Application.Quit();

        // If running in the Unity Editor, stop Play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
