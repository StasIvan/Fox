using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float maxHitPoints = 5;
    public float startingHitPoints = 1;
    public abstract void ResetCharacter();
    public abstract IEnumerator DamageCharacter(float damage, float interval);
    public virtual void KillCharacter()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
    public virtual IEnumerator FlickerCharacter()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
