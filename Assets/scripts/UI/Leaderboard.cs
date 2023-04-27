using LootLocker.Requests;
using UnityEngine;
using System.Collections;
using TMPro;

// Leaderboard is submits scores and writes them on the leaderboard.
public class Leaderboard : MonoBehaviour
{
    public static Leaderboard instance;

    public TMP_Text[] names = new TMP_Text[10];
    public TMP_Text[] scores = new TMP_Text[10];

    // LoginRoutine logs in to lootlocker with an identifier.
    // In the main menu the device identifier is used while the worldData identifier is used whilst playing and saving scores.
    public static IEnumerator LoginRoutine(string identifier)
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession(identifier, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("player id", response.player_id.ToString());
                SetPlayerName();
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    // SubmitScoreRoutine submits score with the acount currently logged in.
    public static IEnumerator SubmitScoreRoutine()
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("player id");
        string leaderboardKey = "leaderboard";
        int score = GetScore();
        LootLockerSDKManager.SubmitScore(playerID, score, leaderboardKey, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    // Setts the player name for the guest acount and therfore on the leaderboard.
    public static void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(GameManager.playerName, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Succesfully set player name");
            }
            else
            {
                Debug.Log("Could not set player name" + response.Error);
            }
        });
    }

    // GetScore convertes the time in the TimeHandler to a score.
    public static int GetScore()
    {
        if (TimeHandler.instance != null)
        {
            int score = 10000 * TimeHandler.instance.day;
            score += (int)(10000f * (TimeHandler.instance.time - 6) / 24f);
            return score;
        }
        else
        {
            Debug.LogError("can't get score there is no time handler");
            return -1;
        }
    }

    // GetTime converts a score to a time.
    public static (int days, float hours) GetTime(int score)
    {
        float time = score / 10000f;
        int days = Mathf.FloorToInt(time);
        float hours = (time - days) * 24;
        return (days, hours);
    }

    // Awake is called when script instance is loaded.
    private void Awake()
    {
        instance = this;
    }

    // FetchTopHighscoresRoutine fetches top 10 highscores and dispays them.
    public IEnumerator FetchTopHighscoresRoutine()
    {
        string leaderboardKey = "leaderboard";
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardKey, 10, 0, (response) =>
        {
            if (response.success)
            {

                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < 10; i++)
                {
                    if (members.Length <= i)
                    {
                        names[i].text = "";
                        scores[i].text = "";
                        continue;
                    }
                    names[i].text = members[i].rank + ". ";
                    if (members[i].player.name != "")
                    {
                        names[i].text += members[i].player.name;
                    }
                    else
                    {
                        names[i].text += members[i].player.id;
                    }
                    (int days, float hours) = GetTime(members[i].score);
                    scores[i].text = days.ToString() + " days " + Mathf.Round(hours).ToString() + " hours";
                }
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    // OnEnable is called when object is enabled and active and here it fetches the highscorse.
    private void OnEnable()
    {
        StartCoroutine(FetchTopHighscoresRoutine());
    }

}
