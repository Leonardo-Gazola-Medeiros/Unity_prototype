using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 12f;
    public float jumpForce = 7f;
    public float groundCheckDistance = 0.2f;

    private Animator animator;
    private Rigidbody rb;
    private bool isGrounded;
    private float currentSpeed;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        // Handle Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
            animator.SetBool("Sprint", true);
        }
        else
        {
            currentSpeed = moveSpeed;
            animator.SetBool("Sprint", false);
        }

        // Movement or Stop
        if (movement.magnitude > 0)
        {
            Move(movement);
        }
        else
        {
            // Stop immediately when no input
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Animator updates
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetBool("Jump", !isGrounded);

        // Attack
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void Move(Vector3 direction)
    {
        Vector3 moveDir = transform.TransformDirection(direction) * currentSpeed;
        moveDir.y = rb.linearVelocity.y; // keep gravity effect
        rb.linearVelocity = moveDir;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }
}