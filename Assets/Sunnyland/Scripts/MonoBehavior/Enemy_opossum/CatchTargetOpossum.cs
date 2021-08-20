using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTargetOpossum : MonoBehaviour
{
    FollowPlayer _follow;
    private void Start()
    {
        _follow = GetComponent<FollowPlayer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _follow.isTarget = true;
            _follow.AttackPlayer(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _follow.isTarget = false;
    }
}
