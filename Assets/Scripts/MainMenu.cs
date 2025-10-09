using UnityEngine;
using UnityEngine.SceneManagement; // Needed to load scenes

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    public string firstLevelName = "MapScene"; // Replace with your actual first level scene name

    // Called when Start Button is clicked
    public void StartGame()
    {
        SceneManager.LoadScene(firstLevelName);
    }

    // Called when Options Button is clicked
    public void OpenOptions()
    {
        Debug.Log("Options menu not implemented yet");
        // You can later show an Options panel or load another scene
    }

    // Called when Quit Button is clicked
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit(); // Works in build, not in editor
    }
}
