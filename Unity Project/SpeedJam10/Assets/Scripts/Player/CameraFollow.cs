using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform Player;
    public SplineContainer PathSpline;
    public float CameraDistance = 8f;
    public float CameraHeight = 4f;
    public float LookAheadDistance = 0.1f; // How far ahead on spline to look

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (Player == null || PathSpline == null)
        {
            return;
        }

        // Get player's position on spline
        float playerSplinePos = Player.GetComponent<PlayerMovement>().GetSplinePosition();

        // Position camera behind player
        float cameraSplinePos = playerSplinePos - (CameraDistance / PathSpline.CalculateLength());
        cameraSplinePos = (cameraSplinePos + 1f) % 1f; // Handle wrap-around

        PathSpline.Evaluate(cameraSplinePos, out float3 cameraPos, out float3 forward, out float3 up);

        // Offset camera upward and backward
        Vector3 finalCameraPos = (Vector3)cameraPos + (((Vector3)up).normalized * CameraHeight);

        // Look ahead on the spline
        float lookAheadPos = (playerSplinePos + (LookAheadDistance / PathSpline.CalculateLength())) % 1f;
        PathSpline.Evaluate(lookAheadPos, out float3 lookTarget, out _, out _);
        lookTarget.y = finalCameraPos.y;

        transform.position = finalCameraPos;
        transform.LookAt(lookTarget, up);
    }

    float GetSplinePosition(Vector3 worldPos)
    {
        // Find closest point on spline (simplified)
        SplineUtility.GetNearestPoint(PathSpline.Spline, worldPos, out float3 nearest, out float t);
        return t;
    }
}
