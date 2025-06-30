using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Obstacle : MonoBehaviour
{
    public SplineContainer MyPath = null;
    public bool StartFromWorldPos = true;
    public float MoveSpeed = 0.0f;
    public Vector3 PathOffset = Vector3.zero;

    // -1 means always active
    public int ActiveLap = -1;

    float progress = 0.0f;
    bool currentlyActive = false;
    SpriteRenderer sprite;
    Collider coll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider>();
        //StartNewLap(-1);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewLap(int lap)
    {
        currentlyActive = (lap == ActiveLap) || (ActiveLap == -1);
        sprite.enabled = currentlyActive;
        coll.enabled = currentlyActive;
    }
}
