using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float MaxSpeed = 10f;
    public float StartSpeed = 10f;
    public float Acceleration = 5.0f;
    public float XSpeed = 5.0f;
    public float XLeftBoundary = 1.0f;
    public float XRightBoundary = 1.0f;
    public float3 SplinePositionOffset = Vector3.zero;
    float currMaxSpeed = 0.0f;
    float currSpeed = 0.0f;

    public float JumpStrength = 9.0f;
    public float GravityStrength = -9.81f;
    private float jumpOffset = 0.0f;
    private float yVel = 0.0f;

    public float SlideLength = 1.0f;
    public float SlideCooldown = 0.2f;
    private float slideTimer = 0.0f;
    private float slideCD = 0.0f;

    [Header("Spline Reference")]
    public SplineContainer PathSpline;

    private float splinePosition = 0f; // 0 to 1 along the spline
    private float xOffset = 0f;


    private Animator anim;
    private PlayerCollision playerCollision;

    public void SetSplinePosition(float newPos)
    {
        splinePosition = newPos;
    }

    public float GetSplinePosition()
    {
        return splinePosition;
    }

    public bool IsJumping()
    {
        return yVel != 0.0f || jumpOffset != 0.0f;
    }

    public bool IsSliding()
    {
        return slideTimer > 0.0f;
    }

    public float GetMaxSpeed()
    {
        return currMaxSpeed;
    }

    public void SetMaxSpeed(float newMaxSpeed)
    {
        currMaxSpeed = newMaxSpeed;
    }

    public float GetCurrSpeed()
    {
        return currSpeed;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerCollision = GetComponent<PlayerCollision>();
        currMaxSpeed = MaxSpeed;
        currSpeed = StartSpeed; 
    }

    void Update()
    {
        HandleSliding();
        HandleJumping();
        MoveAlongSpline();
        HandleLaneSwitching();
        UpdatePosition();
    }

    void HandleSliding()
    {
        if (IsSliding())
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0.0f)
            {
                playerCollision.SetDefaultCollider();
                anim.SetTrigger("Run");
            }

            return;
        }

        slideCD -= Time.deltaTime;

        if (!IsJumping() && slideCD <= 0.0f && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftShift)))
        {
            slideTimer = SlideLength;
            slideCD = SlideCooldown;
            playerCollision.SetSlideCollider();
            anim.SetTrigger("Slide");
        }
    }

    void HandleJumping()
    {
        if (jumpOffset == 0.0f && yVel == 0.0f)
        {
            if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !IsSliding())
            {
                yVel = JumpStrength;
                playerCollision.SetJumpCollider();
                anim.SetTrigger("Jump");
            }

            return;
        }

        yVel += GravityStrength * Time.deltaTime;
        jumpOffset += yVel * Time.deltaTime;
        if(jumpOffset <= 0.0f)
        {
            jumpOffset = 0.0f;
            yVel = 0.0f;
            playerCollision.SetDefaultCollider();
            anim.SetTrigger("Run");
        }
    }

    void MoveAlongSpline()
    {
        currSpeed = Mathf.Min(currSpeed + (Acceleration * Time.deltaTime), currMaxSpeed);

        // Move forward along the spline
        float splineLength = PathSpline.CalculateLength();
        splinePosition += (currSpeed / splineLength) * Time.deltaTime;

        // Keep position in 0-1 range (loops automatically)
        splinePosition = splinePosition % 1f;
    }

    void HandleLaneSwitching()
    {
        if (Input.GetKey(KeyCode.A) && xOffset > -XLeftBoundary)
        {
            xOffset = Mathf.Max(xOffset - (XSpeed * Time.deltaTime), -XLeftBoundary);
        }
        else if (Input.GetKey(KeyCode.D) && xOffset < XRightBoundary)
        {
            xOffset = Mathf.Min(xOffset + (XSpeed * Time.deltaTime), XRightBoundary);
        }
    }

    void UpdatePosition()
    {
        // Get position and direction on spline
        PathSpline.Evaluate(splinePosition, out float3 position, out float3 forward, out float3 up);

        // Calculate right vector for lane offset
        float3 right = math.normalize(math.cross(up, forward));

        // Apply lane offset
        position += right * xOffset;

        position += SplinePositionOffset;

        position.y += jumpOffset;

        // Update transform
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(forward, up);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.GetComponent<Obstacle>())
        {
            anim.SetTrigger("Stumble");
            currSpeed = 0.0f;
        }
    }
}
