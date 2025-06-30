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
    private float laneOffset = 0f;


    private Animator anim;
    private PlayerCollision playerCollision;

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

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerCollision = GetComponent<PlayerCollision>();
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
        //if (anim)
        //{
        //    anim.SetFloat("Blend", IsJumping() ? 1.0f : 0.0f);
        //}

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

        position.y += jumpOffset;

        // Update transform
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(forward, up);
    }
}
