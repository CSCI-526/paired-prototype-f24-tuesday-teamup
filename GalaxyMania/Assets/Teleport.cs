using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public float teleportHeightOffset = 1f; // Height offset after teleporting (optional, to avoid clipping)
    public float colorTolerance = 0.02f; // Tolerance for color matching
    private bool hasTeleported = false;

    // Define your target color
    Color targetColor = new Color(105f / 255f, 45f / 255f, 195f / 255f, 1f);

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
                    // Get the width of the platform from the target object's SpriteRenderer bounds
                    SpriteRenderer targetRenderer = targetObject.GetComponent<SpriteRenderer>();
                    if (targetRenderer != null)
                    {
                        // Teleport the sphere to the target object's position
                        Vector3 targetPosition = targetObject.transform.position;

                        // Get the bounds of the target object to calculate its width
                        float platformWidth = targetRenderer.bounds.size.x;

                        // Adjust the x position to teleport to the right side of the platform
                        targetPosition.x += platformWidth / 2;  // Move to the right side
                        targetPosition.y += teleportHeightOffset; // Move slightly above the platform (optional)

                        // Set the sphere's position to the new target object position
                        transform.position = targetPosition;

                        // Mark the teleport as completed to prevent further teleports
                        hasTeleported = true;
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
