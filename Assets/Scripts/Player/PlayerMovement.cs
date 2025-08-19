using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;
    private Rigidbody rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A-D
        float vertical = Input.GetAxis("Vertical");     // W-S

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        // Move with Rigidbody for physics
        if (movement.magnitude > 0)
        {
            rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);

        }

        // Update animator parameters
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        // Jump (spacebar)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Jump", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("Jump", false);
        }

        // Attack (mouse click)
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }
}
