using UnityEngine;
using Cinemachine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private Rigidbody rb;
    private CinemachineVirtualCamera virtualCamera;

    private bool isSprinting;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode dashKey = KeyCode.Space;

    private bool isDashing;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashDrag = 1.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.LogError("Rigidbody not found on the player.");

        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCamera == null) Debug.LogError("CinemachineVirtualCamera not found in the scene.");

        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("Animator not found on the player.");

        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }

    private void Update()
    {
        HandleSprinting();
        HandleGrounded();
        HandleMovement();
        HandleDashing();
    }

    private void HandleSprinting()
    {
        if (Input.GetKeyDown(sprintKey) && isGrounded)
        {
            isSprinting = true;
        }
        else if (Input.GetKeyUp(sprintKey) || !isGrounded)
        {
            isSprinting = false;
        }

        animator.SetBool("IsSprinting", isSprinting);
    }

    private void HandleGrounded()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, whatIsGround);
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(x, 0f, y).normalized;

        float speed = isSprinting ? sprintSpeed : walkSpeed;

        bool isMoving = inputDirection.magnitude >= 0.1f;
        animator.SetBool("IsWalking", isMoving);

        if (isMoving)
        {
            Vector3 cameraForward = virtualCamera.transform.forward;
            cameraForward.y = 0;  
            Vector3 cameraRight = virtualCamera.transform.right;
            cameraRight.y = 0;  

            Vector3 moveDirection = (cameraForward * y + cameraRight * x).normalized;

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            MovePlayer(moveDirection, speed);
        }
    }

    private void MovePlayer(Vector3 direction, float speed)
    {
        Vector3 movement = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);
    }

    private void HandleDashing()
    {
        if (Input.GetKeyDown(dashKey) && !isDashing && isGrounded)
        {
            isDashing = true;
            animator.SetBool("IsDashing", true);
            animator.SetTrigger("DashTrigger");
            StartCoroutine(Dash());
        }
        else if (!Input.GetKeyDown(dashKey) || !isGrounded)
        {
            animator.SetBool("IsDashing", false);
        }
    }

    private IEnumerator Dash()
    {
        float startTime = Time.time;
        Vector3 dashDirection = transform.forward;

        rb.velocity = dashDirection * dashSpeed;

        while (Time.time < startTime + dashDuration)
        {
            rb.AddForce(-rb.velocity.normalized * dashDrag, ForceMode.Acceleration);
            yield return null;
        }

        rb.velocity = Vector3.zero;
        isDashing = false;
    }
}














