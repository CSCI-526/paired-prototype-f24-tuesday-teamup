using System;
using UnityEngine;

public class HorizontalPlatformController : MonoBehaviour
{
    public float rotationThreshold = 180f; // Angle threshold to change states
    private Renderer squareRenderer;
    private BoxCollider2D boxCollider;
    private Transform levelParent;

    private void Awake()
    {
        squareRenderer = GetComponent<Renderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        levelParent = transform.parent; // Assuming the square is a child of the rotating level
    }

    void Update()
    {
        if (levelParent != null)
        {
            float currentRotation = levelParent.rotation.eulerAngles.z; // Get the current rotation of the level

            // Determine if the square should be active based on the rotation
            if (currentRotation >= -rotationThreshold && currentRotation <= rotationThreshold)
            {
                SetActiveState(true); // Active state for the first half of rotation
            }
            else
            {
                SetActiveState(false); // Inactive state for the second half of rotation
            }
        }
    }

    public void SetActiveState(bool isActive)
    {
        // Change color and collider state based on the active state
        if (isActive)
        {
            squareRenderer.material.color = new Color32(59, 180, 89, 255); // Active color
            EnableCollider(true);
        }
        else
        {
            squareRenderer.material.color = new Color(1, 0, 0, 0.5f); // Transparent red
            EnableCollider(false);
        }
    }

    private void EnableCollider(bool enable)
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = enable;
        }
        else if (enable)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }
    }
}
