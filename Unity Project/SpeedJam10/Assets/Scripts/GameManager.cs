using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    int currLap = 0;
    float gameTimer = 0.0f;

    bool paused = false;
    bool gameStarted = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currLap = 0;
        gameStarted = true;
        UpdateObstacles();
    }

    // Update is called once per frame
    void Update()
    {
        if (paused || !gameStarted)
        {
            return;
        }

        gameTimer += Time.deltaTime;
    }

    public float GetGameTimer()
    {
        return gameTimer;
    }

    public int GetLap()
    {
        return currLap;
    }

    public void CompleteLap()
    {
        ++currLap;
        UpdateObstacles();
    }

    public void SetPaused(bool newPaused)
    {
        paused = newPaused;
    }

    void UpdateObstacles()
    {
        LapObstacleEnabler[] obstacles = FindObjectsByType<LapObstacleEnabler>(FindObjectsSortMode.None);
        foreach (LapObstacleEnabler obstacle in obstacles)
        {
            obstacle.OnNewLap(currLap);
        }
    }
}
