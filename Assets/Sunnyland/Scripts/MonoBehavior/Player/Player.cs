using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    public float hitPoints;
    public float quantityCoin = 0;
    Animator animator;
    [HideInInspector] public BoxCollider2D _body;
    [SerializeField] private HealthBar healthBarPrefap;
    private HealthBar HealthBar;
    Rigidbody2D _rb2D;
    MovementController controller;
    
    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _body = GetComponent<BoxCollider2D>();
        controller = GetComponent<MovementController>();
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;
            if (hitObject != null)
            {
                bool shouldDisappear = false;
                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:

                        quantityCoin += hitObject.quantity;
                        shouldDisappear = true;
                        break;
                    case Item.ItemType.HEALTH:
                        if (hitPoints < maxHitPoints)
                        {
                            hitPoints = hitPoints + hitObject.quantity;
                            shouldDisappear = HealthBar.AddItem(hitPoints);
                        }
                        break;
                    default:
                        break;
                }
                if (shouldDisappear)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }

    public override IEnumerator DamageCharacter(float damage, float interval) 
    {
        while (true)
        {

            StartCoroutine(FlickerCharacter());
            hitPoints = hitPoints - damage;
            
            animator.SetBool("isHurt", true);
            if (controller.dodgeCoroutine != null)
            {
                controller.StopCoroutine(controller.dodgeCoroutine);
            }
            controller.playerJump(controller.jumpForce / 1.5f);

            controller.StartCoroutine(controller.waitToControllPlayer(0.5f));
            if (hitPoints >= float.Epsilon)
            {
                HealthBar.DeleteItem();

                break;
            }
            if (hitPoints <= float.Epsilon)
            {
                animator.SetBool("isDeath", true);
                
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
     
    void quitGame()
    {
        Application.Quit();
    }

    public void destroyBox()
    {
        
        _body.enabled = false;
        _rb2D.simulated = false;
    }
    
    public void stopHurt()
    {
        if (hitPoints > float.Epsilon)
        {
            animator.SetBool("isHurt", false);
        }
    }

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
        HealthBar = Instantiate(healthBarPrefap);
        HealthBar.character = this;
    }

    private void OnEnable()
    {
        ResetCharacter();
    }

    public void LoadLevelScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
