using UnityEngine;

public class LevelRotation : MonoBehaviour
{
    public float rotationSpeed = 60f;
    public Transform levelParent;
    public Camera mainCamera;

    private float currentRotation = 0f;
    private Vector2 lastPlayerPosition;
    private float targetRotation = 0f;

    void Start()
    {
        lastPlayerPosition = transform.position;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        targetRotation += -horizontalInput * rotationSpeed * Time.deltaTime;
        currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * 5f);

        // Apply rotation to the level parent
        levelParent.rotation = Quaternion.Euler(0f, 0f, currentRotation);

        // Update last player position
        lastPlayerPosition = transform.position;

        // Adjust camera to keep entire level visible
        AdjustCamera();
    }

    void AdjustCamera()
    {
        // Calculate the bounds of the rotated level
        Bounds levelBounds = CalculateLevelBounds();

        // Calculate the required orthographic size to fit the rotated level
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = levelBounds.size.x / levelBounds.size.y;

        if (screenRatio >= targetRatio)
        {
            mainCamera.orthographicSize = levelBounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            mainCamera.orthographicSize = levelBounds.size.y / 2 * differenceInSize;
        }

        // Center camera
        mainCamera.transform.position = new Vector3(levelBounds.center.x, levelBounds.center.y, mainCamera.transform.position.z);
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