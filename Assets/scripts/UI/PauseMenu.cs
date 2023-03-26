using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

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
}
