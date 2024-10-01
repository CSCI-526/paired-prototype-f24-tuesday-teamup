using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public float teleportHeightOffset = 1f; // Height offset after teleporting (optional, to avoid clipping)
    public float colorTolerance = 0.02f; // Tolerance for color matching
    public float moveThreshold = 5f; // Distance required to reset teleport
    private bool hasTeleported = false;
    private Vector3 lastTeleportedPosition; // Store position after teleport

    // Define your target color
    Color targetColor = new Color(105f / 255f, 45f / 255f, 195f / 255f, 1f);

    void Update()
    {
        // Check if the object has moved far enough from the last teleported position to reset teleportation
        if (hasTeleported && Vector3.Distance(transform.position, lastTeleportedPosition) > moveThreshold)
        {
            hasTeleported = false; // Reset the teleportation flag
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the SpriteRenderer of the object the sphere collided with
        SpriteRenderer objectRenderer = collision.gameObject.GetComponent<SpriteRenderer>();

        if (objectRenderer != null && !hasTeleported)
        {
            Color objectColor = objectRenderer.color; 

            // Check if the object's color matches the target color
            if (IsColorClose(objectColor, targetColor, colorTolerance))
            {
                // Find another object with the same specific color
                GameObject targetObject = FindObjectWithColor(targetColor, collision.gameObject);

                if (targetObject != null)
                {
                    // Get the SpriteRenderer and bounds of the target object
                    SpriteRenderer targetRenderer = targetObject.GetComponent<SpriteRenderer>();
                    if (targetRenderer != null)
                    {
                        // Get the right side of the platform in world space
                        Vector3 platformRightSide = targetRenderer.bounds.max; // Get the rightmost point in world space
                        
                        // Calculate the direction the platform is facing (right direction of the platform)
                        Vector3 platformRightDirection = targetObject.transform.right; // Local right direction (based on rotation)

                        // Offset the teleportation position towards the right side of the platform
                        Vector3 targetPosition = platformRightSide + (platformRightDirection * (targetRenderer.bounds.size.x / 2));
                        targetPosition.y += teleportHeightOffset; // Adjust height to avoid clipping into the platform

                        // Teleport the object (sphere) to the calculated position
                        transform.position = targetPosition;

                        // Mark the teleport as completed and store the last teleported position
                        hasTeleported = true;
                        lastTeleportedPosition = transform.position;
                    }
                }
            }
        }
    }

    GameObject FindObjectWithColor(Color color, GameObject currentObject)
    {
        Debug.Log("Searching for an object with the target color...");

        // Find all SpriteRenderers in the scene
        SpriteRenderer[] allSpriteRenderers = FindObjectsOfType<SpriteRenderer>();

        // Loop through the SpriteRenderers to find an object with the same specific color, excluding the current one
        foreach (SpriteRenderer spriteRenderer in allSpriteRenderers)
        {
            if (spriteRenderer.gameObject != currentObject) // Exclude the current object
            {
                if (IsColorClose(spriteRenderer.color, color, colorTolerance)) // Check if colors match
                {
                    return spriteRenderer.gameObject; // Found a matching object
                }
            }
        }
        return null; // No matching object found
    }

    bool IsColorClose(Color32 color1, Color32 color2, float tolerance)
    {
        // Compare the colors using the RGB values and a tolerance
        return Mathf.Abs(color1.r - color2.r) / 255f <= tolerance &&
               Mathf.Abs(color1.g - color2.g) / 255f <= tolerance &&
               Mathf.Abs(color1.b - color2.b) / 255f <= tolerance;
    }
}
