using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField]TMP_Text text;
    private void OnEnable()
    {
        Time.timeScale = 0f;
        float result = Globals.timeHandler.day + Globals.timeHandler.time / 24 - 1.25f;
        text.text = "you survived: " + Mathf.Floor(result) + " days and " + Mathf.Round(((result % 1) * 24)).ToString() + "houres";
    }

    public void Retry()
    {
        SceneManager.LoadScene("World Scene");
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
