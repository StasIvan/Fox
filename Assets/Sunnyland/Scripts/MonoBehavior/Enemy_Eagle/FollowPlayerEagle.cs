using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FollowPlayerEagle : MonoBehaviour
{
    WanderEagle wander;
    public float pursuitSpeed;
    public bool followPlayer;
    public Coroutine _catchPlayer;
    Collider2D collision = null;
    public LayerMask layerPlayer;
    [HideInInspector]public Vector3 newEndPoint;
    public bool isTarget;
    public float remainingDistance = 0;
    bool exitTarget;
    
    private void Start()
    {
        
        wander = GetComponent<WanderEagle>();
        //StartCoroutine(enemySight());
        
    }
    private void Update()
    {
        checkDistance();
        enemySight();
    }

    void enemySight()
    {

        if (collision != null)
        {
            exitTarget = true;
            startFollowTarget(collision);
        }
        else if (!isTarget && exitTarget)
        {
            exitTarget = false;
            stopFollowTarget();
        }

    }
    void checkDistance()
    {
        Vector2 vector = new Vector2(wander.endPosition.x - transform.position.x, wander.endPosition.y - transform.position.y);
        remainingDistance = Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2));
        
        if(remainingDistance <= float.Epsilon)
        {
            stopFollowTarget();
        }
    }

    void startFollowTarget(Collider2D collision)
    {
       
        Player player = collision.gameObject.GetComponent<Player>();
        newEndPoint = new Vector3(player.transform.position.x, player._body.bounds.max.y);
        wander.currentSpeed = pursuitSpeed;

        if (wander.targetTransform == null)
        {

            wander.targetTransform = collision.gameObject.transform;
            wander.targetPosition = newEndPoint;

        }
        
        if (wander.moveCoroutine != null)
        {
            StopCoroutine(wander.moveCoroutine);
        }
        wander.moveCoroutine = StartCoroutine(wander.Move(wander.rb2d, wander.currentSpeed));

    }

    void stopFollowTarget()
    {

        //wander.currentSpeed = wander.wanderSpeed;

        if (wander.moveCoroutine != null)
        {
            StopCoroutine(wander.moveCoroutine);
        }
        wander.targetTransform = null;
        wander.targetPosition = new Vector3(9999, 9999);

    }
    
}
