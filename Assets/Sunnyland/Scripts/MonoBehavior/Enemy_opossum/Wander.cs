using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class Wander : MonoBehaviour
{
    public float wanderSpeed;
    [HideInInspector] public float currentSpeed;
    float directionChangeInterval;
    public bool followPlayer;
    [HideInInspector] public Coroutine moveCoroutine;
    [HideInInspector] public Rigidbody2D rb2d;
    Vector3 endPosition;
    [HideInInspector] public Transform targetTransform = null;

    private void Start()
    {
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
            moveCoroutine = StartCoroutine(Move());
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    void ChooseNewEndpoint()
    {
        endPosition = new Vector3(Random.Range(transform.position.x - 2, transform.position.x + 2), 0.42f);
    }


    public IEnumerator Move()
    {
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;
        directionChangeInterval = remainingDistance / currentSpeed;
        while (remainingDistance > float.Epsilon)
        {
            if (targetTransform != null)
            {
                endPosition = targetTransform.position;
                endPosition.y = transform.position.y;
            }

            if (rb2d != null)
            {

                Vector3 newPosition = Vector3.MoveTowards(rb2d.position, endPosition, currentSpeed * Time.fixedDeltaTime);

                if (transform.position.x > endPosition.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                this.rb2d.MovePosition(newPosition);

                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }

    }

    private void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }
}
