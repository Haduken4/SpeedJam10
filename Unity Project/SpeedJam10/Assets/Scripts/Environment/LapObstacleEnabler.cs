using UnityEngine;
using System.Collections.Generic;

public class LapObstacleEnabler : MonoBehaviour
{
    public int ActiveLap = -1; // -1 = always active

    List<GameObject> obstacleSplines = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            obstacleSplines.Add(transform.GetChild(i).gameObject);
        }

        //OnNewLap(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNewLap(int lap)
    {
        foreach(GameObject spline in obstacleSplines)
        {
            spline.SetActive(lap == ActiveLap || ActiveLap == -1);
        }
    }
}
