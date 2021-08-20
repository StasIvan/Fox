using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTarget : MonoBehaviour
{
    FollowPlayerEagle2 _follow;
    private void Start()
    {
        _follow = GetComponentInParent<FollowPlayerEagle2>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _follow._isTarget = true;
            _follow.enemySight(collision.gameObject);
            _follow._isTarget = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _follow._isTarget = false;
    }
}
