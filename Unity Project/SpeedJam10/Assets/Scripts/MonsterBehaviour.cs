using UnityEngine;

public class MonsterBehaviour : MonoBehaviour
{
    public PlayerMovement Movement = null;
    public PlayerRespawn Respawn = null;
    public float Speed = 2.5f;
    public float SpeedMultiplier = 0.1f;
    public float GameOverLocalYThreshold = -0.7f;
    public float DefaultY = -0.9f;

    public AudioPlayer DeadPlayer = null;
    public float DeadYMax = 0.5f;
    public float DeadSpeed = 5.0f;

    public float StartTimer = 2.0f;
    float startTimer = 0.0f;

    bool dead = false;
    bool fading = false;

    private void Start()
    {
        startTimer = StartTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer > 0.0f)
        {
            startTimer -= Time.deltaTime;
            return;
        }

        if (!Movement || !Respawn)
        {
            return;
        }

        Vector3 localPos = transform.localPosition;

        if (!Respawn.IsRespawning() && dead)
        {
            dead = false;
            startTimer = StartTimer / 2.0f;
        }

        if (dead)
        {
            localPos.y = Mathf.Min(localPos.y + DeadSpeed * Time.deltaTime, DeadYMax);
            if (Respawn.IsFadingOut() || fading)
            {
                fading = true;
                localPos.y = DefaultY;
            }

            transform.localPosition = localPos;
            return;
        }

        float speed = Mathf.Max(Speed - Movement.GetCurrSpeed(), 0) * SpeedMultiplier;
        
        localPos.y += speed * Time.deltaTime;
        if (localPos.y >= GameOverLocalYThreshold)
        {
            Respawn.StartRespawn(true);
            fading = false;
            dead = true;
            DeadPlayer.PlayAudio();
        }

        transform.localPosition = localPos;
    }
}
