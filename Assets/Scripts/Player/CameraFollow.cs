using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;    // the player
    public Transform headTarget; // Optional: the player's head for better aim targeting
    
    [Header("Camera Position")]
    public Vector3 offset = new Vector3(0, 2, -5); // camera distance from player
    public float smoothSpeed = 0.125f;
    
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float maxLookUpAngle = 80f;
    public float maxLookDownAngle = 80f;
    
    private float verticalRotation = 0f;

    void LateUpdate()
    {
        if (target == null) return;

        HandleMouseLook();
        FollowTarget();
    }

    private void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate camera horizontally around the player
        target.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookDownAngle, maxLookUpAngle);
    }

    private void FollowTarget()
    {
        // Calculate rotation with vertical angle
        Quaternion rotation = Quaternion.Euler(verticalRotation, target.eulerAngles.y, 0);

        // Calculate desired position based on rotated offset
        Vector3 rotatedOffset = rotation * offset;
        Vector3 desiredPosition = target.position + rotatedOffset;

        // Smooth camera movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Look at target (or head if assigned)
        Transform lookTarget = headTarget != null ? headTarget : target;
        transform.LookAt(lookTarget.position + Vector3.up * 1.5f); // Slight upward offset for better framing
    }
}