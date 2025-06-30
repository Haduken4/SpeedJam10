using UnityEngine;
using TMPro;

public class FinalText : MonoBehaviour
{
    public string PreText = "";
    public TextMeshProUGUI ScoreText = null;
    public bool IntSeconds = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float timeScore = GameManager.Instance.GetGameTimer();
        int scoreMins = (int)((timeScore - (timeScore % 60.0f)) / 60.0f);
        float scoreSeconds = timeScore - (scoreMins * 60.0f);
        string minString = scoreMins.ToString("00");
        string secondsString = scoreSeconds.ToString("00:000");
        if (IntSeconds)
        {
            secondsString = ((int)scoreSeconds).ToString("00");
        }
        ScoreText.text = PreText + minString + ":" + secondsString; ;
    }
}
