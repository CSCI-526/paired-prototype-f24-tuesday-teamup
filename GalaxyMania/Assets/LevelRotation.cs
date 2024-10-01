using System.Collections;
using UnityEngine;

public class LevelRotation : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public Transform levelParent;
    public Transform antiRotatPlatforms;
    public Camera mainCamera;
    public float zoomDuration = 3f; // Time it takes to zoom in to the player

    private float currentRotation = 0f;
    private Vector2 lastPlayerPosition;
    private float targetRotation = 0f;
    private float targetZoom; // The target orthographic size (player-focused zoom)
    private bool isZooming = true;

    void Start()
    {
        lastPlayerPosition = transform.position;

        // Start the camera fully zoomed out, showing a large portion of the level
        float fullLevelHeight = CalculateLevelBounds().size.y;
        mainCamera.orthographicSize = fullLevelHeight * 1f; // Show a large view of the level (1.5x the height)

        // Set the target zoom to still show a good portion of the level
        float levelHeight = fullLevelHeight;
        float visibilityRatio = 0.9f; // 70% of the level will be visible after zoom
        targetZoom = (levelHeight / 2) * visibilityRatio;

        // Start the coroutine to smoothly zoom in on the player
        StartCoroutine(SmoothZoomIn());
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        targetRotation += -horizontalInput * rotationSpeed * Time.deltaTime;
        currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * 5f);

        // Apply rotation to the level parent
        levelParent.rotation = Quaternion.Euler(0f, 0f, currentRotation);

        // Apply the opposite rotation to the AntiRotatPlatforms
        foreach (Transform platform in antiRotatPlatforms)
        {
            // Rotate each platform around its own origin in the opposite direction
            platform.Rotate(Vector3.forward * rotationSpeed * horizontalInput * Time.deltaTime);
        }
    }

    void Update()
    {
        if (!isZooming)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            targetRotation += -horizontalInput * rotationSpeed * Time.deltaTime;
            currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * 5f);

            // Apply rotation to the level parent
            levelParent.rotation = Quaternion.Euler(0f, 0f, currentRotation);

            // Apply the opposite rotation to the AntiRotatPlatforms
            foreach (Transform platform in antiRotatPlatforms)
            {
                platform.Rotate(Vector3.forward * rotationSpeed * horizontalInput * Time.deltaTime);
            }

            // Update last player position
            lastPlayerPosition = transform.position;

            // Adjust camera to keep focus on the player
            AdjustCamera();
        }
    }

    void AdjustCamera()
    {
        // Calculate the vertical offset to place the player at 75% from the bottom
        float verticalOffset = mainCamera.orthographicSize * 0.5f;

        // Focus the camera on the player with the vertical offset applied
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + verticalOffset, mainCamera.transform.position.z);
    }

    IEnumerator SmoothZoomIn()
    {
        // Zoom from the full view to the target zoom over zoomDuration seconds
        float elapsedTime = 0f;
        float initialZoom = mainCamera.orthographicSize;

        while (elapsedTime < zoomDuration)
        {
            // Smoothly interpolate the orthographic size from the full level view to the target zoom
            mainCamera.orthographicSize = Mathf.Lerp(initialZoom, targetZoom, elapsedTime / zoomDuration);

            // Keep the camera focused on the player while zooming in
            AdjustCamera();

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Set the final orthographic size to the target value
        mainCamera.orthographicSize = targetZoom;
        isZooming = false; // End the zooming process
    }

    Bounds CalculateLevelBounds()
    {
        Bounds bounds = new Bounds(levelParent.position, Vector3.zero);
        Renderer[] renderers = levelParent.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }
}
