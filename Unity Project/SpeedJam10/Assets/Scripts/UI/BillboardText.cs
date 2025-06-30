using UnityEngine;

public class BillboardText : MonoBehaviour
{
    private Camera playerCamera;

    void Start()
    {
        // Find the main camera or player camera
        playerCamera = Camera.main;
        if (playerCamera == null)
            playerCamera = FindFirstObjectByType<Camera>();
    }

    void LateUpdate()
    {
        if (playerCamera != null)
        {
            // Make the canvas face the camera
            transform.LookAt(playerCamera.transform);
            transform.Rotate(0, 180, 0); // Flip to face the right direction
        }
    }
}