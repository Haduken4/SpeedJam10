using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardCard : MonoBehaviour
{
    public int Place = 1;
    public TextMeshProUGUI NameText = null;
    public TextMeshProUGUI ScoreText = null;

    LeaderboardManager lm = null;

    LeaderboardData data = new LeaderboardData();

    float timer = 0.5f;
    int tries = 0;

    // Start is called before the first frame update
    void Start()
    {
        lm = FindFirstObjectByType<LeaderboardManager>();
        data.success = false;
        data.score = 0;
        data.name = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!lm.Loaded())
        {
            NameText.text = "Leaderboard Can't Connect";
            ScoreText.text = "";

            return;
        }
        else if (data.success == false && tries < 10)
        {
            //???
            timer -= Time.unscaledDeltaTime;
            if (timer <= 0.0f)
            {
                lm.GetScore(Place, data);
                timer = Random.Range(0.4f, 0.6f);
                ++tries;
            }
        }

        NameText.text = data.name;

        float timeScore = data.score / 100.0f;
        int scoreMins = (int)((timeScore - (timeScore % 60.0f)) / 60.0f);
        float scoreSeconds = timeScore - (scoreMins * 60.0f);
        ScoreText.text = $"{scoreMins}:{scoreSeconds}";
    }

    public void Refresh()
    {
        lm.GetScore(Place, data);
        tries = 0;
    }
}
