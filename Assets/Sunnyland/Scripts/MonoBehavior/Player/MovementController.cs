using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField]float movementSpeed = 5.0f;
    public float jumpForce = 100f;
    float dodgespeed = 15.0f;
    bool isGrounded;
    bool isJump;
    bool jump = false;
    [SerializeField]Transform groundCheck;
    [SerializeField] float checkRadius;
    public LayerMask whatIsGround;
    bool _playerControl;
    Vector2 _movement = new Vector2();
    Animator _animator = null;
    Rigidbody2D _rb2D;
    bool _dodge;
    bool _readyToDodge;
    float _directionChangeInterval;
    public Coroutine dodgeCoroutine;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb2D = GetComponent<Rigidbody2D>();
        _playerControl = true;
        _readyToDodge = true;
    }
    
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        
        _movement.y = Input.GetAxis("Vertical");

        if (_movement.y >= 0) 
        {
            _movement.x = Input.GetAxis("Horizontal");
        }
        
        else
        {
            _movement.x = 0;
        }
        _movement.Normalize();
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
        {
            jump = true;
        }
        UpdateState();
        if ((Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.LeftShift)) && _readyToDodge)
        {
            _dodge = true;
        }

    }

    void FixedUpdate()
    {
        if (_playerControl)
        {
            MoveController();
        }
        
    }

    void MoveController()
    {
        
        if (_movement.y >=0)
        {
            _rb2D.velocity = new Vector2(_movement.x * movementSpeed, _rb2D.velocity.y);
        }
        
        if (!Mathf.Approximately(_movement.x, 0))
        {
            
            transform.localScale = new Vector3(Mathf.Sign(_movement.x), 1, 1);
        }
            
        if (jump)
        {

            playerJump(jumpForce);
        }

        jump = false;

        if (_dodge)
        {
            dodgeCoroutine = StartCoroutine(dodgeController(dodgespeed));
        }
        _dodge = false;
    }
    
    IEnumerator dodgeController(float speed)
    {
        Vector3 startPosition = transform.position;
        float percentComplete = 0.0f;
        Vector3 endPosition = new Vector3(transform.position.x + 1.5f * transform.localScale.x, transform.position.y);
        Vector2 vector = new Vector2(endPosition.x - transform.position.x, endPosition.y - transform.position.y);
        float remainingDistance = Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2));
        _directionChangeInterval = (remainingDistance / dodgespeed);
        StartCoroutine(enabledBody(_directionChangeInterval));
        while (percentComplete <= 1.0f)
        {
            percentComplete += Time.fixedDeltaTime * speed;
            _rb2D.MovePosition(Vector3.Lerp(startPosition, endPosition, percentComplete));
            percentComplete += Time.fixedDeltaTime / speed;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(waitForNextDodge());
    }
    
    IEnumerator enabledBody(float directionChangeInterval)
    {
        //_body.enabled = false;
        float oldGravityScale = _rb2D.gravityScale;
        _rb2D.gravityScale = 0;
        yield return new WaitForSeconds(directionChangeInterval);
        //_body.enabled = true;
        _rb2D.gravityScale = oldGravityScale;
    }

    IEnumerator waitForNextDodge()
    {
        _readyToDodge = false;
        yield return new WaitForSeconds(0.7f);
        _readyToDodge = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyOpossum") || collision.gameObject.CompareTag("EnemyEagle"))
        {

            if (collision.transform.position.y + 0.2f < transform.position.y)
            {
                playerJump(jumpForce);
            }
        }
    }
    
    public IEnumerator waitToControllPlayer(float second)
    {
        _playerControl = false;
        
        yield return new WaitForSeconds(second);
        
        _playerControl = true;
    }

    public void playerJump(float jumpForce)
    {

        _rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        if (_rb2D.velocity.y > 9.3)
        {
            Vector3 newVelocity = Vector3.ClampMagnitude(_rb2D.velocity, 9.5f);
            _rb2D.velocity = newVelocity;
        }
        
    }
    
    void UpdateState()
    {
        if (Mathf.Approximately(_movement.x, 0))
        {
            _animator.SetBool("isWalking", false);
        }
        else if (isGrounded)
        {
            _animator.SetBool("isWalking", true);
        }
        if (_rb2D.velocity.y > 0)
        {
            isJump = true;
        }
        else if (_rb2D.velocity.y < 0)
        {
            isJump = false;
        }
        else
        {
            isJump = false;
        }
        if (_movement.y < 0)
        {
            _animator.SetBool("isCrounch", true);
        }
        else
        {
            _animator.SetBool("isCrounch", false);
        }
        
        _animator.SetFloat("xDir", _movement.x);
        _animator.SetBool("isJump", isJump);
        _animator.SetBool("isGrounded", isGrounded);
    }
    
    
    
}
