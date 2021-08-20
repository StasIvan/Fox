using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerEagle2 : MonoBehaviour
{
    WanderEagle2 _wander;
    public LayerMask layerPlayer;
    public Vector3 newEndPoint;
    public bool _isTarget;
    bool _nextAttack;

    private void Start()
    {
        _nextAttack = true;
        _wander = GetComponent<WanderEagle2>();
    }
    private void Update()
    {
        
    }

    public void enemySight(GameObject _collision)
    {
        if (_nextAttack)
        {

            if (_collision != null)
            {
                startFollowTarget(_collision);
            }
            else if (!_isTarget)
            {
                stopFollowTarget();
            }
        }
    }

    IEnumerator WaitForNextAttack()
    {
        _nextAttack = false;
        yield return new WaitForSeconds(2.0f);
        _nextAttack = true;
    }

    void startFollowTarget(GameObject collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        newEndPoint = player.transform.position;
        Vector2 vector = new Vector2(newEndPoint.x - transform.position.x, newEndPoint.y - transform.position.y);
        _wander.currentSpeed = Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2));
        if (_wander.moveCoroutine != null)
        {
            _wander.StopCoroutine(_wander.moveCoroutine);
        }
        if (_wander.attackCoroutine != null)
        {
            _wander.StopCoroutine(_wander.attackCoroutine);
        }
        _wander.attackCoroutine = StartCoroutine(_wander.attackMove(newEndPoint, _wander.currentSpeed));
        
    }

    public void stopFollowTarget()
    {
        if (_wander.attackCoroutine != null)
        {
            _wander.StopCoroutine(_wander.attackCoroutine);
            
            _wander.attackCoroutine = null;
            StartCoroutine(WaitForNextAttack());
        }
        
    }
    

}
