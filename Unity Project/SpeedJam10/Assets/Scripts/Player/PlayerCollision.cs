using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Vector3 DefaultColliderScale = Vector3.one;
    public Vector3 DefaultColliderOffset = Vector3.zero;
    public bool SetDefaultsOnStart = false;

    public Vector3 JumpColliderScale = Vector3.one;
    public Vector3 JumpColliderOffset = Vector3.zero;

    public Vector3 SlideColliderScale = Vector3.one;
    public Vector3 SlideColliderOffset = Vector3.zero;

    BoxCollider boxCollider = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (SetDefaultsOnStart)
        {
            DefaultColliderScale = boxCollider.size;
            DefaultColliderOffset = boxCollider.center;
        }
    }

    public void SetDefaultCollider()
    {
        boxCollider.size = DefaultColliderScale;
        boxCollider.center = DefaultColliderOffset;
    }

    public void SetJumpCollider()
    {
        boxCollider.size = JumpColliderScale;
        boxCollider.center = JumpColliderOffset;
    }

    public void SetSlideCollider()
    {
        boxCollider.size = SlideColliderScale;
        boxCollider.center = SlideColliderOffset;
    }
}
