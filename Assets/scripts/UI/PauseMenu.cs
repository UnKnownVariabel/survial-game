using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;


    private void OnEnable()
    {
        Globals.pause = true;
    }

    private void OnDisable()
    {
        Globals.pause = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Resume()
    {
        gameObject.SetActive(false);
    }

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
