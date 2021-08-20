using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public float hitPoints ;
    public static Animator animator;
    
    [HideInInspector] public BoxCollider2D _box;
    Rigidbody2D rb2D;
    
    public float damageStrength;
    Coroutine damageCoroutine;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isLive", true);
        
        _box = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }
   
    public override IEnumerator DamageCharacter(float damage, float interval)
    {
        while (true)
        {
            
            StartCoroutine(FlickerCharacter());
            hitPoints = hitPoints - damage;
            if (hitPoints <= float.Epsilon)
            {
                animator.SetBool("isLive", false);
                
                
                break;
            }
            if (interval >= float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }

        }
    }
    public void destroyBox()
    {
        Destroy(_box);
        rb2D.simulated = false;
    }
    
    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;

    }
    private void OnEnable()
    {
        ResetCharacter();
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            bool willHurtPlayer = _box.bounds.min.x >= player._body.bounds.max.x || _box.bounds.max.x <= player._body.bounds.min.x;
            if (damageCoroutine == null && willHurtPlayer && player.hitPoints >= 0) 
            {
                damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 2.0f));
                
            }
            
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
                
            }
        }
    }

}
