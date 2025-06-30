using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardButton : MonoBehaviour
{
    public TMP_InputField NameField = null;
    //public TextMeshProUGUI StatusText = null;


    bool submitted = false;
    LeaderboardManager lm = null;

    bool cheated = false;

    // Start is called before the first frame update
    void Awake()
    {
        lm = FindFirstObjectByType<LeaderboardManager>();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (cheated)
        {
            return;
        }

        if (!submitted)
        {
            //StatusText.text = StatusText.text;
        }
        else
        {
            if (!lm.Loaded())
            {
                //StatusText.text = "Failed to connect to leaderboard";
                return;
            }
            else if (lm.Thinking())
            {
                //StatusText.text = "Processing...";
            }
            else if (!lm.Succeeded())
            {
                //StatusText.text = "Failed to send score to leaderboard";

                submitted = false;
                GetComponent<Button>().interactable = true;
            }
            else
            {
                //StatusText.text = "Score uploaded to leaderboard!";

                // Refresh leaderboard
                LeaderboardCard[] cards = FindObjectsByType<LeaderboardCard>(FindObjectsSortMode.None);
                foreach (LeaderboardCard card in cards)
                {
                    card.Refresh();
                }

                submitted = false;
            }
        }
    }

    public void SubmitScore()
    {
        if (NameField.text.Length != 0)
        {
            GlobalGameData.PlayerName = NameField.text;
            lm.SubmitScore((int)(GameManager.Instance.GetGameTimer() * 100));
            submitted = true;
        }
        else
        {
            //StatusText.text = "You must submit a name";
        }

        // Lock button
        GetComponent<Button>().interactable = false;
    }
}
