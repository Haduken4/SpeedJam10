using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public float RespawnTime = 5.0f;
    public float FadeOutStartTime = 1.0f;
    public float FadeOutTime = 1.0f;
    public float AnimationStartTime = 2.0f;
    public float PlayerSpriteVisibleTime = 5.0f;
    public GameObject BedAnimationPrefab = null;
    public SpriteRenderer FadeOutObject = null;
        
    PlayerMovement movement = null;
    SpriteRenderer sr = null;

    bool respawning = false;
    bool spawnedAnimation = false;
    bool doFade = false;
    bool fadingOut = false;
    float timer = 0.0f;
    float lastMaxSpeed = 0.0f;

    public bool IsRespawning()
    {
        return respawning;
    }

    public bool IsFadingOut()
    {
        return fadingOut;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();

        StartRespawn();
    }

    // Update is called once per frame
    void Update()
    {
        if(respawning)
        {
            timer += Time.deltaTime;

            if(timer >= FadeOutStartTime && doFade)
            {
                Debug.Log("Fading");
                fadingOut = true;
                Color c = Color.clear;
                c.a = 1.0f - ((timer - FadeOutStartTime) / FadeOutTime);
                FadeOutObject.color = c;
            }

            if (timer >= FadeOutStartTime + FadeOutTime && fadingOut)
            {
                FadeOutObject.color = Color.clear;
                fadingOut = false;
            }

            if(timer >= AnimationStartTime && !spawnedAnimation)
            {
                Instantiate(BedAnimationPrefab, transform.position, transform.rotation);
                spawnedAnimation = true;
            }

            if (timer >= PlayerSpriteVisibleTime && !sr.enabled)
            {
                sr.enabled = true;
            }

            if (timer >= RespawnTime)
            {
                respawning = false;
                movement.SetMaxSpeed(lastMaxSpeed);
            }
        }
    }

    public void StartRespawn(bool fade = false)
    {
        respawning = true;
        spawnedAnimation = false;
        timer = 0.0f;
        lastMaxSpeed = movement.GetMaxSpeed();
        movement.SetMaxSpeed(0.0f);
        movement.SetSplinePosition(0.0f);
        sr.enabled = false;

        if (fade)
        {
            fadingOut = false;
            doFade = true;
        }
        else
        {
            timer += Mathf.Min(FadeOutStartTime + FadeOutTime, AnimationStartTime);
            fadingOut = false;
            doFade = false;
        }
    }
}
