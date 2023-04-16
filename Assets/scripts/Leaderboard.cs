using LootLocker.Requests;
using UnityEngine;
using System.Collections;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard instance;
    public TMP_Text playerNames;
    public TMP_Text playerScores;
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

    public static int GetScore()
    {
        if (Globals.timeHandler != null)
        {
            int score = 10000 * Globals.timeHandler.day;
            score += (int)(10000f * (Globals.timeHandler.time - 6) / 24f);
            return score;
        }
        else
        {
            Debug.LogError("can't get score there is no time handler");
            return -1;
        }
    }

    public static (int days, float hours) GetTime(int score)
    {
        float time = score / 10000f;
        int days = Mathf.FloorToInt(time);
        float hours = (time - days) * 24;
        return (days, hours);
    }

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        StartCoroutine(FetchTopHighscoresRoutine());
    }

    public IEnumerator FetchTopHighscoresRoutine()
    {
        string leaderboardKey = "leaderboard";
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardKey, 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Names\n\n";
                string tempPlayerScores = "Times\n\n";

                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id;
                    }
                    (int days, float hours) = GetTime(members[i].score);
                    tempPlayerScores += days.ToString() + " days " + Mathf.Round(hours).ToString() + " hours\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    

}
