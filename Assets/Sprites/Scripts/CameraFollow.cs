using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // The player to follow
    public Vector3 offset = new Vector3(0, 2, -10); // Camera offset from player
    
    [Header("Follow Settings")]
    public float smoothSpeed = 5f; // How smoothly the camera follows (higher = smoother)
    public bool followX = true; // Whether to follow player horizontally
    public bool followY = true; // Whether to follow player vertically
    
    [Header("Boundaries")]
    public bool useBoundaries = false; // Whether to limit camera movement
    public float minX = -10f; // Minimum X position
    public float maxX = 10f;  // Maximum X position
    public float minY = -5f;  // Minimum Y position
    public float maxY = 5f;   // Maximum Y position
    
    [Header("Look Ahead")]
    public bool useLookAhead = false; // Whether to look ahead in movement direction
    public float lookAheadDistance = 2f; // How far ahead to look
    public float lookAheadSmoothSpeed = 3f; // How smoothly to look ahead
    
    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    private Vector3 lookAheadOffset = Vector3.zero;
    
    void Start()
    {
        // If no target is assigned, try to find the player automatically
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log("Camera automatically found player: " + player.name);
            }
            else
            {
                Debug.LogWarning("CameraFollow: No target assigned and no GameObject with 'Player' tag found!");
            }
        }
        
        // Set initial position
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // Calculate desired position
        desiredPosition = target.position + offset;
        
        // Apply look ahead if enabled
        if (useLookAhead)
        {
            ApplyLookAhead();
            desiredPosition += lookAheadOffset;
        }
        
        // Apply boundaries if enabled
        if (useBoundaries)
        {
            desiredPosition = ApplyBoundaries(desiredPosition);
        }
        
        // Smoothly move camera
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
    
    void ApplyLookAhead()
    {
        // Get player's movement direction
        Rigidbody2D playerRb = target.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector3 movementDirection = playerRb.linearVelocity.normalized;
            
            // Calculate look ahead offset
            Vector3 targetLookAhead = movementDirection * lookAheadDistance;
            
            // Smoothly interpolate to the target look ahead
            lookAheadOffset = Vector3.Lerp(lookAheadOffset, targetLookAhead, lookAheadSmoothSpeed * Time.deltaTime);
        }
    }
    
    Vector3 ApplyBoundaries(Vector3 position)
    {
        return new Vector3(
            Mathf.Clamp(position.x, minX, maxX),
            Mathf.Clamp(position.y, minY, maxY),
            position.z
        );
    }
    
    // Method to set target at runtime
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    // Method to change offset at runtime
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
    
    // Method to enable/disable boundaries at runtime
    public void SetBoundaries(bool enabled, float minXVal = -10f, float maxXVal = 10f, float minYVal = -5f, float maxYVal = 5f)
    {
        useBoundaries = enabled;
        if (enabled)
        {
            minX = minXVal;
            maxX = maxXVal;
            minY = minYVal;
            maxY = maxYVal;
        }
    }
    
    // Draw boundaries in Scene view for easy editing
    void OnDrawGizmosSelected()
    {
        if (useBoundaries)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(
                new Vector3((minX + maxX) / 2, (minY + maxY) / 2, transform.position.z),
                new Vector3(maxX - minX, maxY - minY, 1)
            );
        }
        
        // Draw camera offset
        if (target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(target.position + offset, 0.5f);
            Gizmos.DrawLine(target.position, target.position + offset);
        }
    }
}

