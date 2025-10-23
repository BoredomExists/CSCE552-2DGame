using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager for the win/lose scene buttons
/// </summary>
public class MenuManager : MonoBehaviour
{
    // Function for button to go back to the main menu
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
