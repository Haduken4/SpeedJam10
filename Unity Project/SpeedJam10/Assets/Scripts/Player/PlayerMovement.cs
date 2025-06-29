using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float laneWidth = 2f;
    public int currentLane = 1; // 0=left, 1=center, 2=right

    [Header("Spline Reference")]
    public SplineContainer pathSpline;

    private float splinePosition = 0f; // 0 to 1 along the spline
    private float laneOffset = 0f;

    void Update()
    {
        MoveAlongSpline();
        HandleLaneSwitching();
        UpdatePosition();
    }

    void MoveAlongSpline()
    {
        // Move forward along the spline
        float splineLength = pathSpline.Spline.GetLength();
        splinePosition += (speed / splineLength) * Time.deltaTime;

        // Keep position in 0-1 range (loops automatically)
        splinePosition = splinePosition % 1f;
    }

    void HandleLaneSwitching()
    {
        if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
        {
            currentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentLane < 2)
        {
            currentLane++;
        }

        // Smoothly interpolate to target lane
        float targetOffset = (currentLane - 1) * laneWidth;
        laneOffset = Mathf.Lerp(laneOffset, targetOffset, Time.deltaTime * 8f);
    }

    void UpdatePosition()
    {
        // Get position and direction on spline
        pathSpline.Evaluate(splinePosition, out float3 position, out float3 forward, out float3 up);

        // Calculate right vector for lane offset
        float3 right = math.normalize(math.cross(up, forward));

        // Apply lane offset
        position += right * laneOffset;

        // Update transform
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(forward, up);
    }
}
