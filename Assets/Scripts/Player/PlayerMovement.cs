using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float groundCheckDistance = 0.2f;

    private Animator animator;
    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Settings for Rigidbody
        rb.constraints = RigidbodyConstraints.FreezeRotation; // prevent tipping over
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A-D
        float vertical = Input.GetAxis("Vertical");     // W-S

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        // Movement (handled in FixedUpdate for physics)
        if (movement.magnitude > 0)
        {
            Move(movement);
        }

        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f);

        // Jump (spacebar) 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Jump", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("Jump", false);
        }

        // Animator updates
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetBool("Jump", !isGrounded);

        // Sprint (Shift)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("Sprint", true);
        }
        else
        {
            animator.SetBool("Sprint", false);
        }

        // Attack (mouse click)
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void Move(Vector3 direction)
    {
        // Convert movement relative to player’s forward
        Vector3 moveDir = transform.TransformDirection(direction) * moveSpeed;
        moveDir.y = rb.linearVelocity.y; // keep gravity effect
        rb.linearVelocity = moveDir;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }
}
