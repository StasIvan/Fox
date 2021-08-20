using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEagle2 : Character
{
    public float hitPoints;
    public static Animator animator;


    [HideInInspector] public BoxCollider2D _body;
    Rigidbody2D rb2D;
    
    public float damageStrength;
    Coroutine damageCoroutine;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isLive", true);
        _body = GetComponent<BoxCollider2D>();
       
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

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }

    private void OnEnable()
    {
        ResetCharacter();
    }

    public void destroyBox()
    {
        _body.enabled = false;
        rb2D.simulated = false;
    }

    IEnumerator enabledBox()
    {
        _body.enabled = false;
        yield return new WaitForSeconds(1.0f);
        _body.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            bool willHurtPlayer = player._body.bounds.center.y >= _body.bounds.max.y; ;

            if (damageCoroutine == null && !willHurtPlayer && player.hitPoints >= 0)
            {
                damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 2.0f));
                StartCoroutine(enabledBox());
            }
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (damageCoroutine != null)
            {
                
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
                
            }
        }
    }
}
