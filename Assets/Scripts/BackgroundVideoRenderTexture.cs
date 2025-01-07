using UnityEngine;
using UnityEditor; // Needed to access the Editor API

public class ExitButtonScript : MonoBehaviour
{
    public ExitButtonScript()
    {
    }

    // This method will be called when the Exit button is clicked
    public void OnExitButtonClicked()
    {
        #if UNITY_EDITOR
        // Stop play mode in the Unity editor
        EditorApplication.isPlaying = false;
        #else
        // If you are running a build, use Application.Quit()
        Application.Quit();
        #endif
    }
}
