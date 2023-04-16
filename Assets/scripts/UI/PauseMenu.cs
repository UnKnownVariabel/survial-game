using UnityEngine;
using UnityEngine.SceneManagement;

// Pausemenu is attached to the pause menu and contains the funcitons for its buttons.
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;

    // OnEnable is called when object is enabled and active.
    private void OnEnable()
    {
        Globals.pause = true;
    }

    // OnEnable is called when object is disabled.
    private void OnDisable()
    {
        Globals.pause = false;
    }

    // Opens main menu.
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // Resumes game.
    public void Resume()
    {
        gameObject.SetActive(false);
    }

    // Goes back from settings to paus menu.
    public void Back()
    {
        menu.SetActive(true);
        settings.SetActive(false);
    }

    public void Settings()
    {
        menu.SetActive(false);
        settings.SetActive(true);
    }
}
