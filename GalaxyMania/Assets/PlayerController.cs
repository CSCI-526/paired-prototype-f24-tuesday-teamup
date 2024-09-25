using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    public float speed = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform player;
    public float jump = 15f;
    public Transform levelParent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        CheckIfGrounded();
        ApplyCustomGravity();
    }

    void Move()
    {
        moveInput = Input.GetAxis("Horizontal"); 
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
        }
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 1f, groundLayer);
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

}
