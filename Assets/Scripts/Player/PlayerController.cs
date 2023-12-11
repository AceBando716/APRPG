using UnityEngine;
using Cinemachine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public int playerLevel = 1;
    [SerializeField] public int currentExp = 0;
    [SerializeField] public int expToNextLevel = 100;
    [SerializeField] public int maxLevel = 100;
    [SerializeField] public float damageMultiplier = 1.0f; // Initial damage multiplier
    [SerializeField] private float baseDamage = 10f; // Initial base damage
    private Animator animator;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private Rigidbody rb;
    private CinemachineVirtualCamera virtualCamera;

    private int attackPhase = 0;

 
 
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
        HandleAttack();

        if (currentExp >= expToNextLevel)
        {
            LevelUp();

        }
    }

    public int GetCurrentExp()
    {
        return currentExp;
    }

     
    public int GetExpToNextLevel()
    {
        return expToNextLevel;
    }

     
    public int GetPlayerLevel()
    {
        return playerLevel;
    }
    public void GainExperience(int amount)
    {
        currentExp += amount;

    }
    private void LevelUp()
    {
        if (playerLevel < maxLevel)
        {
            playerLevel++;
            currentExp -= expToNextLevel;  
            expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.1f);
            damageMultiplier += 0.1f; // 10%  
        }
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

        float movementMagnitude = inputDirection.magnitude;

        float speed = 0f; // Idle
        if (movementMagnitude >= 0.1f) // Walking
        {
            speed = 0.5f; // Walking speed
            if (isSprinting) // Sprinting
            {
                speed = 1f; // Sprinting speed
            }
        }

        animator.SetFloat("Speed", speed);

        if (movementMagnitude >= 0.1f)
        {
            Vector3 cameraForward = virtualCamera.transform.forward;
            cameraForward.y = 0;  
            Vector3 cameraRight = virtualCamera.transform.right;
            cameraRight.y = 0;  

            Vector3 moveDirection = (cameraForward * y + cameraRight * x).normalized;

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            MovePlayer(moveDirection, (isSprinting ? sprintSpeed : walkSpeed));
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
        animator.SetTrigger("DashTrigger"); // Make sure "DashTrigger" is the correct name in the Animator
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
    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Responding to left mouse button click
        {
            

            float damage = baseDamage * damageMultiplier;
            animator.SetTrigger("AttackTrigger");
            
             
        }

         
         
    }

}
 


