using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float Speed = 10f;
    public float LaneWidth = 2f;
    public int CurrentLane = 1; // 0=left, 1=center, 2=right
    public float3 SplinePositionOffset = Vector3.zero;

    [Header("Spline Reference")]
    public SplineContainer PathSpline;

    private float splinePosition = 0f; // 0 to 1 along the spline
    private float laneOffset = 0f;

    public float GetSplinePosition()
    {
        return splinePosition;
    }

    void Update()
    {
        MoveAlongSpline();
        HandleLaneSwitching();
        UpdatePosition();
    }

    void MoveAlongSpline()
    {
        // Move forward along the spline
        float splineLength = PathSpline.CalculateLength();
        splinePosition += (Speed / splineLength) * Time.deltaTime;

        // Keep position in 0-1 range (loops automatically)
        splinePosition = splinePosition % 1f;
    }

    void HandleLaneSwitching()
    {
        if (Input.GetKeyDown(KeyCode.A) && CurrentLane > 0)
        {
            CurrentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.D) && CurrentLane < 2)
        {
            CurrentLane++;
        }

        // Smoothly interpolate to target lane
        float targetOffset = (CurrentLane - 1) * LaneWidth;
        laneOffset = Mathf.Lerp(laneOffset, targetOffset, Time.deltaTime * 8f);
    }

    void UpdatePosition()
    {
        // Get position and direction on spline
        PathSpline.Evaluate(splinePosition, out float3 position, out float3 forward, out float3 up);

        // Calculate right vector for lane offset
        float3 right = math.normalize(math.cross(up, forward));

        // Apply lane offset
        position += right * laneOffset;

        position += SplinePositionOffset;

        // Update transform
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(forward, up);
    }
}
