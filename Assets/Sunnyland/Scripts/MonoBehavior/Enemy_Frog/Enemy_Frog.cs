using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : MonoBehaviour
{
    private float movementSpeed;
    private float jumpForce;
    bool isGrounded;
    bool isJump;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    Vector2 movement = new Vector2();
    private BoxCollider2D _box;
    public static Animator animator;
    public static Rigidbody2D rb2D;

    void Start()
    {
        _box = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        JumpController();
        UpdateState();
    }

    public void JumpController()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        movementSpeed = Random.Range(2.5f, 5f);
        jumpForce = Random.Range(5, 8);
        movement.x = Random.Range(-1, 1);
        movement.Normalize();
        rb2D.velocity = new Vector2(movement.x * movementSpeed, rb2D.velocity.y);
        if (!Mathf.Approximately(movement.x, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(movement.x), 1, 1);
        }
        
        if (isGrounded && movement.x != 0 )
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        
    }
    void UpdateState()
    {
        
        if (rb2D.velocity.y > 0)
        {
            isJump = true;
        }
        else if (rb2D.velocity.y < 0)
        {
            isJump = false;
        }
        
        
        animator.SetBool("isJump", isJump);
        animator.SetBool("isGrounded", isGrounded);
    }

}
