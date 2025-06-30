using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class LeaderboardData
{
    public int score;
    public string name;
    public bool success = false;
}

public class LeaderboardManager : MonoBehaviour
{
    public string Leaderboard = "Classic Set";

    bool loaded = false;
    bool thinking = false;
    bool succeeded = false;

    string playerID = "";

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("LootLockerID"))
        {
            playerID = PlayerPrefs.GetString("LootLockerID");
            LootLockerSDKManager.StartGuestSession(playerID, (response) =>
            {
                if (!response.success)
                {
                    Debug.Log(response.text);

                    // Failed to start session
                    loaded = false;
                    return;
                }

                playerID = response.player_identifier;
                PlayerPrefs.SetString("LootLockerID", playerID);
                loaded = true;
            });
        }
        else
        {
            LootLockerSDKManager.StartGuestSession((response) =>
            {
                if (!response.success)
                {
                    Debug.Log(response.text);

                    // Failed to start session
                    loaded = false;
                    return;
                }

                playerID = response.player_identifier;
                PlayerPrefs.SetString("LootLockerID", playerID);
                loaded = true;
            });
        }
    }

    public bool SubmitScore(int score)
    {
        if (!loaded || thinking)
        {
            return false;
        }

        thinking = true;
        LootLockerSDKManager.SetPlayerName(GlobalGameData.PlayerName, (response) => { });
        LootLockerSDKManager.SubmitScore(playerID, score, Leaderboard, GlobalGameData.PlayerName, SubmitResponse);

        return true;
    }

    void SubmitResponse(LootLockerSubmitScoreResponse response)
    {
        thinking = false;
        succeeded = response.success;
    }

    public void GetScore(int place, LeaderboardData dataOut)
    {
        LootLockerSDKManager.GetScoreList(Leaderboard, place, (response) =>
        {
            if (!response.success)
            {
                Debug.Log(response.text);
                dataOut.success = false;
            }
            else if (dataOut != null && response.items != null)
            {
                if (response.items.Length > place - 1)
                {
                    dataOut.name = response.items[place - 1].player.name;
                    dataOut.score = response.items[place - 1].score;
                    dataOut.success = true;

                    //Debug.Log(response.text);
                }
            }
        });
    }

    public bool Loaded()
    {
        return loaded;
    }

    public bool Thinking()
    {
        return thinking;
    }

    public bool Succeeded()
    {
        return succeeded;
    }
}
