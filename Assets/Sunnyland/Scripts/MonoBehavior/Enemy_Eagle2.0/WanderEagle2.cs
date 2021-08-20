using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderEagle2 : MonoBehaviour
{
    public float wanderSpeed;
    public float currentSpeed;
    float _directionChangeInterval;
    [HideInInspector]public Vector2 startPosition;
    public Coroutine moveCoroutine;
    public Coroutine attackCoroutine;
    Rigidbody2D _rb2d;
    FollowPlayerEagle2 _follow;
    Vector3 _endPosition;
    
    private void Start()
    {
        _follow = GetComponent<FollowPlayerEagle2>();
        startPosition = transform.position;
        currentSpeed = wanderSpeed;
        _rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(WanderRoutine());
        _endPosition = transform.position;
    }

    public IEnumerator WanderRoutine()
    {
        while (true)
        {
            if (attackCoroutine == null)
            {
                ChooseNewEndpoint();
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);
                }

                moveCoroutine = StartCoroutine(Move(_rb2d, currentSpeed));

            }
            yield return new WaitForSeconds(_directionChangeInterval);
        }
    }

    void ChooseNewEndpoint()
    {
        _endPosition = new Vector3(Random.Range(transform.position.x - 2, transform.position.x + 2), startPosition.y);
        if (Mathf.Abs(transform.position.y - startPosition.y) < 0.1f)
        {
            currentSpeed = wanderSpeed;
        }
    }

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        
        Vector2 vector = new Vector2(_endPosition.x - transform.position.x, _endPosition.y - transform.position.y);
        float remainingDistance = Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2));
        _directionChangeInterval = (remainingDistance / speed);

        while (remainingDistance > float.Epsilon)
        {
            if (rigidBodyToMove != null)
            {

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, _endPosition, speed * Time.fixedDeltaTime);

                if (transform.position.x > _endPosition.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                _rb2d.MovePosition(newPosition);
                vector = new Vector2(_endPosition.x - transform.position.x, _endPosition.y - transform.position.y);
                remainingDistance = Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2));
                
            }
            
            yield return new WaitForFixedUpdate();
        }

    }
    
    public IEnumerator attackMove(Vector3 targetPlayer, float speed)
    {
        Vector3 startPosition = transform.position;
        float percentComplete = 0.0f;
        _endPosition = new Vector3(targetPlayer.x + (targetPlayer.x - transform.position.x), startPosition.y);
        if (transform.position.x > _endPosition.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        while (percentComplete <= 1.0f)
        {
            percentComplete += Time.fixedDeltaTime / speed;
            float currentHeight = Mathf.Sin(Mathf.PI * percentComplete)/ targetPlayer.y;
            _rb2d.MovePosition( Vector3.Lerp(startPosition, _endPosition, percentComplete) + Vector3.down * currentHeight);
            percentComplete += Time.fixedDeltaTime / speed;
            yield return new WaitForFixedUpdate();
        }
        if (Mathf.Abs(transform.position.y - _endPosition.y) < 0.1f) 
        {
            _rb2d.velocity = new Vector2(0, 0);
            _follow.stopFollowTarget();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") )
        {
            
            if (attackCoroutine != null)
            {
                _follow.stopFollowTarget();
            }
        }
    }

    /*private void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }*/
}
