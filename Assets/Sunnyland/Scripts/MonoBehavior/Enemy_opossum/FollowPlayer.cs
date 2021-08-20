using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    
    Wander wander;
    public float pursuitSpeed;
    public bool followPlayer;
    [HideInInspector] public Player player;
    public GameObject collision;
    public LayerMask layerPlayer;
    public bool isTarget;
    bool exitTarget;
    public float checkRadius;
    private void Start()
    {
        wander = GetComponentInParent<Wander>();
    }

    private void Update()
    {
        
    }
    public void AttackPlayer(GameObject collision)
    {
        if (isTarget && followPlayer)
        {
            wander.currentSpeed = pursuitSpeed;
            wander.targetTransform = collision.gameObject.transform;
            if (wander.moveCoroutine != null)
            {
                StopCoroutine(wander.moveCoroutine);
            }
            wander.moveCoroutine = StartCoroutine(wander.Move());
            exitTarget = true;
        }
        
        else if (!isTarget && exitTarget)
        {
            wander.currentSpeed = wander.wanderSpeed;
            exitTarget = false;
            if (wander.moveCoroutine != null)
            {
                StopCoroutine(wander.moveCoroutine);
            }
            wander.targetTransform = null;
        }
    }
}
