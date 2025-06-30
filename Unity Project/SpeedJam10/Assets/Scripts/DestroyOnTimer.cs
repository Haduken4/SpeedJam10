using UnityEngine;

public class DestroyOnTimer : MonoBehaviour
{
    public float Timer = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, Timer);
    }
}
