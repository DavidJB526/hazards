using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float maxSpeed, jumpForceForward, jumpForceUp, bulletSpeed, recoilSpeed;
    [SerializeField]
    private Collider2D groundDetectTrigger;
    [SerializeField]
    private ContactFilter2D groundContactFilter;


    private Rigidbody2D rb2d;
    private Collider2D[] groundHitDetectionResults = new Collider2D[16];
    private bool grounded;
    private bool isDead = false;
    private bool isFacingRight = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        isFacingRight = true;
        isDead = false;
    }

    private void Update()
    {
        if (!isDead)
        {
            GroundCheck();
        }        
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
            Jump();
        }                                   
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        if (grounded)
        {
            rb2d.AddForce(Vector2.right * movement, ForceMode2D.Impulse);
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
        }

        if (moveHorizontal > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveHorizontal < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Jump()
    {

        if (Input.GetButtonDown("Jump") && grounded && !isDead)
        {
            rb2d.AddForce(Vector2.up * jumpForceUp, ForceMode2D.Impulse);

            if (isFacingRight)
            {
                rb2d.AddForce(Vector2.right * jumpForceForward);
            }
            else if (!isFacingRight)
            {
                rb2d.AddForce(Vector2.left * jumpForceForward);
            }
        }
    }

    private void GroundCheck()
    {
        grounded = groundDetectTrigger.OverlapCollider(groundContactFilter, groundHitDetectionResults) > 0;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
