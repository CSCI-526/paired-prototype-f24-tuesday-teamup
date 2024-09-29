using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinMessageHandler : MonoBehaviour
{
    public TextMeshProUGUI winText; // Reference to the UI Text element
    public Color targetColor = new Color(111f / 255f, 41f / 255f, 26f / 255f, 1f); // Set target color
    public float colorTolerance = 0.02f; // Tolerance for color matching
    private bool wonGame = false;

    private void Start()
    {
        // Initially hide the win message
        winText.text = "";
    }

    void Update()
    {
        if (Input.anyKeyDown && wonGame)
        {
            // Reload the current active scene
            Time.timeScale = 1f;  // Unfreeze the game
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            wonGame = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the SpriteRenderer of the object we collided with
        SpriteRenderer objectRenderer = collision.gameObject.GetComponent<SpriteRenderer>();

        if (objectRenderer != null)
        {
            Color objectColor = objectRenderer.color;

            // Check if the object's color matches the target color
            if (IsColorClose(objectColor, targetColor, colorTolerance))
            {
                // Display the win message
                winText.text = "You Win!";
                Time.timeScale = 0; // Optionally pause the game
                wonGame = true;
            }
        }
    }

    private bool IsColorClose(Color color1, Color color2, float tolerance)
    {
        return Mathf.Abs(color1.r - color2.r) <= tolerance &&
               Mathf.Abs(color1.g - color2.g) <= tolerance &&
               Mathf.Abs(color1.b - color2.b) <= tolerance;
    }
}
