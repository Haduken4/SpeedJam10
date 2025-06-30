using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public float RespawnTime = 3.0f;
    public float AnimationStartTime = 1.0f;
    public float PlayerSpriteVisibleTime = 3.0f;
    public GameObject BedAnimationPrefab = null;
        
    PlayerMovement movement = null;
    SpriteRenderer sr = null;

    bool respawning = false;
    bool spawnedAnimation = false;
    float timer = 0.0f;
    float lastMaxSpeed = 0.0f;

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

    public void StartRespawn()
    {
        respawning = true;
        spawnedAnimation = false;
        timer = 0.0f;
        lastMaxSpeed = movement.GetMaxSpeed();
        movement.SetMaxSpeed(0.0f);
        sr.enabled = false;
    }
}
