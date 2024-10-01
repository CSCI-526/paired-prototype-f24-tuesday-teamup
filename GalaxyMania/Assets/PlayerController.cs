using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject gameOverCanvas;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    public float speed = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform player;
    public float jump = 20f;
    public Transform levelParent;
    private bool isGameOver = false; // Track if the game is over

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameOverCanvas.SetActive(false);  // Ensure the Game Over screen is hidden at the start
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            // Normal game behavior
            Move();
            Jump();
            CheckIfGrounded();
            ApplyCustomGravity();
            CheckIfPlayerFell();
        }
        else
        {
            // Game is over, wait for key press to restart
            CheckForRestart();
        }
    }


    void Move()
    {
        moveInput = Input.GetAxis("Horizontal"); 
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void Jump()
    {
        Debug.Log(isGrounded);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
        }
    }

    void CheckIfPlayerFell()
    {
        // If the player falls below a certain Y position, trigger game over
        if (transform.position.y < -30f)  // Adjust this value depending on your level's height
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;  // Freeze the game
        gameOverCanvas.SetActive(true);  // Show the Game Over screen
        isGameOver = true;  // Mark the game as over
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 2f, groundLayer);
        //Debug.Log("The ground check position is" +groundCheck.position + "and isGrounded is " + isGrounded);
    }

    void ApplyCustomGravity()
    {
        // Get the current rotation angle of the level
        float angle = levelParent.rotation.eulerAngles.z;
        Vector2 gravityDirection;

        // Calculate gravity direction based on the rotation of the level
        if (angle >= 0 && angle <= 90)
        {
            gravityDirection = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), -Mathf.Cos(angle * Mathf.Deg2Rad));
        }
        else
        {
            gravityDirection = new Vector2(-Mathf.Sin(angle * Mathf.Deg2Rad), -Mathf.Cos(angle * Mathf.Deg2Rad));
        }

        // Apply custom gravity force to accelerate the fall as the level rotates
        rb.AddForce(gravityDirection * Physics2D.gravity.magnitude, ForceMode2D.Force);
    }

    void CheckForRestart()
    {
        // If any key is pressed, restart the game
        if (Input.anyKeyDown)
        {
            Time.timeScale = 1f;  // Unfreeze the game
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the current scene
        }
    }

}
