using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float acceleration = 10f;

    [Header("Jump Settings")]
    public float jumpForce = 6f;
    public float gravityMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.25f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevent tipping
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;

        if (groundCheck != null)
            isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
    }

    void FixedUpdate()
    {
        // --- Move ---
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 targetVelocity = moveDir * moveSpeed;
        Vector3 velocity = rb.velocity;

        Vector3 velocityChange = (targetVelocity - new Vector3(velocity.x, 0, velocity.z))
                                * acceleration * Time.fixedDeltaTime;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        // --- Jump ---
        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
        jumpPressed = false;

        // --- Extra gravity for snappier jump feel ---
        if (!isGrounded)
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1f), ForceMode.Acceleration);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
