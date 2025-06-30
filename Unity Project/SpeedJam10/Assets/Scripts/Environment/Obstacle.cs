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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewLap(int lap)
    {
        if (lap == ActiveLap)
        {

        }
    }
}
