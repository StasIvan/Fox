using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderEagle : MonoBehaviour
{
    public float wanderSpeed;
    [HideInInspector] public float currentSpeed;

    float directionChangeInterval;
    public bool followPlayer;
    public Vector2 startPosition;
    //bool attackPlayer;
    [HideInInspector] public Coroutine moveCoroutine;
    [HideInInspector] public Rigidbody2D rb2d;
    FollowPlayerEagle follow;
    [HideInInspector] public Vector3 endPosition;
    [HideInInspector] public Transform targetTransform = null;
    [HideInInspector] public Vector3 targetPosition;
    private void Start()
    {
        follow = GetComponent<FollowPlayerEagle>();
        startPosition = transform.position;
        currentSpeed = wanderSpeed;
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(WanderRoutine());
        endPosition = transform.position;
        
    }

    public IEnumerator WanderRoutine()
    {
        while (true)
        {
            ChooseNewEndpoint();
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    void ChooseNewEndpoint()
    {
        endPosition = new Vector3(Random.Range(transform.position.x - 2, transform.position.x + 2), startPosition.y);
        if (transform.position.y == startPosition.y)
        {
            currentSpeed = wanderSpeed;
        }
    }

    
    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        //float remainingDistance = (transform.position - endPosition).sqrMagnitude;
        
        Vector2 vector = new Vector2(endPosition.x - transform.position.x, endPosition.y - transform.position.y);
        float remainingDistance = Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2));
        directionChangeInterval = (remainingDistance / speed);

        while (remainingDistance > float.Epsilon)
        {
            if (targetTransform != null && (targetPosition.x < 1000 && targetPosition.y < 1000))
            {
                //endPosition = targetTransform.position;
                endPosition = targetPosition;
            }

            if (rigidBodyToMove != null)
            {

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.fixedDeltaTime);

                if (transform.position.x > endPosition.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                rb2d.MovePosition(newPosition);
                vector = new Vector2(endPosition.x - transform.position.x, endPosition.y - transform.position.y);
                remainingDistance = Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2));
                //remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }
            
            yield return new WaitForFixedUpdate();
        }

    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
        {
            currentSpeed = wanderSpeed;
            
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            targetTransform = null;
            targetPosition = new Vector3(9999, 9999);
            
        }
    }
    private void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }
}
