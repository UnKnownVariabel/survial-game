using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// DeathScreen is attached to the deathscreen which apears when the player dies.
public class DeathScreen : MonoBehaviour
{
    [SerializeField]TMP_Text text;

    // OnEnable is called when object is enabled and active.
    private void OnEnable()
    {
        Globals.pause = true;
        float result = TimeHandler.instance.day + (TimeHandler.instance.time - 6) / 24;
        text.text = "you survived: " + Mathf.Floor(result) + " days and " + Mathf.Round(((result % 1) * 24)).ToString() + " hours";
    }
    
    // MainMenu is called from a button to open the main menu.
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // SubmitScore submits time lived to the leaderboard.
    public void SubmitScore()
    {
        StartCoroutine(Leaderboard.SubmitScoreRoutine());
    }
}
