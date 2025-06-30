using UnityEngine;
using TMPro;
public class FinalDoorScript : MonoBehaviour
{
    public TextMeshProUGUI LapText = null;
    public SpriteRenderer FadeObject = null;
    public MonsterBehaviour Monster = null;
    public PlayerMovement Player = null;

    public float WinFadeTime = 2.0f;
    public float WinMonsterFadeTime = 1.0f;
    public float WinMenuTime = 3.0f;
    public GameObject WinMenu = null;
    public int LapsToWin = 3;

    bool winning = false;
    float winTimer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LapText.text = $"0/{LapsToWin}";
    }

    // Update is called once per frame
    void Update()
    {
        if (winning)
        {
            winTimer += Time.deltaTime;
            if(winTimer <= WinMonsterFadeTime)
            {
                SpriteRenderer msr = Monster.GetComponent<SpriteRenderer>();
                Color mColor = msr.color;
                mColor.a = 1.0f - (winTimer / WinMonsterFadeTime);
                msr.color = mColor;
            }
            
            if(winTimer <= WinFadeTime)
            {
                FadeObject.color = new Color(0.75f, 0.77f, 0.8f, (winTimer / WinFadeTime) * 0.9f);
            }

            if (winTimer >= WinMenuTime && WinMenu != null)
            {
                WinMenu.SetActive(true);
                this.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            GameManager.Instance.CompleteLap();
            LapText.text = $"{GameManager.Instance.GetLap()}/{LapsToWin}";

            if (GameManager.Instance.GetLap() >= LapsToWin)
            {
                StartWinSequence();
            }
        }
    }

    void StartWinSequence()
    {
        winning = true;
        winTimer = 0.0f;
        Monster.enabled = false;
        Player.enabled = false;
    }
}
