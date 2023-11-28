using System.Collections;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    public float dashSpeed = 10f;       // Initial speed of the dash
    public float dashDuration = 0.5f;   // Duration of the dash in seconds
    public float dashDrag = 1.5f;       // Drag applied during the dash
    public KeyCode dashKey = KeyCode.LeftControl;

    private Rigidbody rb;
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(dashKey) && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        Vector3 dashDirection = transform.forward;
        float startTime = Time.time;

        // Apply force to initiate the dash
        rb.velocity = dashDirection * dashSpeed;

        while (Time.time < startTime + dashDuration)
        {
            // Simulate deceleration by applying a force in the opposite direction
            rb.AddForce(-rb.velocity.normalized * dashDrag, ForceMode.Acceleration);
            yield return null;
        }

        isDashing = false;
    }
}



