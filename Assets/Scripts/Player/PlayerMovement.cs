using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 720f; // degrees per second

    void Update()
    {
        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Movement direction
        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        if (move.magnitude > 0.1f)
        {
            // Move
            transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

            // Rotate towards movement direction
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}
