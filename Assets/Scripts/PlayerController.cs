using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private Rigidbody rb;
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;

        // Get the Cinemachine Virtual Camera component
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    void FixedUpdate()
    {
        // Get input for movement along the Horizontal and Vertical axes
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(x, 0f, y).normalized;

        // Check if input magnitude is significant enough for movement
        if (inputDirection.magnitude >= 0.1f)
        {
            // Get the camera's forward and right directions
            Vector3 cameraForward = virtualCamera.transform.forward;
            Vector3 cameraRight = virtualCamera.transform.right;

            // Calculate the movement direction based on camera orientation
            Vector3 moveDirection = (cameraForward * y + cameraRight * x).normalized;

            // Rotate the player towards the movement direction
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            // Move the player based on the calculated direction and speed
            MovePlayer(moveDirection);
        }
    }

    void MovePlayer(Vector3 direction)
    {
        // Calculate the movement and apply it to the Rigidbody
        Vector3 movement = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);
    }
}








