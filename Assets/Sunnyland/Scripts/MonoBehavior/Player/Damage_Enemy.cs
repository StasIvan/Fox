using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Enemy : MonoBehaviour
{
    private BoxCollider2D _body;
    float damageInflicted = 1.0f;
    
    Coroutine damageCoroutine = null;
    private void Start()
    {
        _body = GetComponent<BoxCollider2D>();
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character enemy = GetComponent<Character>();
        bool willHurtEnemy = false;
        if (collision.gameObject.CompareTag("EnemyOpossum"))
        {

            enemy = collision.gameObject.GetComponent<Enemy>();
            willHurtEnemy = _body.bounds.center.y >= enemy.GetComponent<Enemy>()._box.bounds.max.y;
        }
        else if (collision.gameObject.CompareTag("EnemyEagle"))
        {
            if (collision.gameObject.GetComponent<EnemyEagle>())
            {
                enemy = collision.gameObject.GetComponent<EnemyEagle>();
                willHurtEnemy = _body.bounds.center.y >= enemy.GetComponent<EnemyEagle>()._body.bounds.max.y;
            }
            else if (collision.gameObject.GetComponent<EnemyEagle2>())
            {
                enemy = collision.gameObject.GetComponent<EnemyEagle2>();
                willHurtEnemy = _body.bounds.center.y >= enemy.GetComponent<EnemyEagle2>()._body.bounds.max.y;
            }

        }
        
        if (willHurtEnemy && damageCoroutine == null)
        {
            damageCoroutine = enemy.StartCoroutine(enemy.DamageCharacter(damageInflicted, 2.0f));
        }
    }
    

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyOpossum") || collision.gameObject.CompareTag("EnemyEagle")) 
        {
            if (damageCoroutine != null)
            {
                //collision.gameObject.GetComponent<Character>().StopCoroutine(damageCoroutine);
                damageCoroutine = null;

            }
        }
       
    }

}

