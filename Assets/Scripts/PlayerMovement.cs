using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState 
{
    WALK,
    ATTACK,
    INTERACT,
    STAGGER,
    IDLE
}

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Vector3 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    public PlayerState currentState;
    public FloatValue currentHealth;
    public SignalSender playerHealthSignal;
    public bool playerInConvo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // set rb to player's rigidBody2D component
        animator = GetComponent<Animator>();
        currentState = PlayerState.WALK;
        playerInConvo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerInConvo)
        {
            moveInput = Vector3.zero;
            moveInput.x = Input.GetAxisRaw("Horizontal"); // get horizontal input (left/right)
            moveInput.y = Input.GetAxisRaw("Vertical"); // get vertical input (up/down)
            moveInput = moveInput.normalized; // gets rid of diagonal movement advantage

            if (Input.GetKeyDown("space") && currentState != PlayerState.ATTACK && currentState != PlayerState.STAGGER)
            {
                // StartCoroutine(AttackCoroutine());
            }
            else if (currentState == PlayerState.WALK || currentState == PlayerState.IDLE)
            {
                UpdateAnimatedMove();
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.ATTACK;
        yield return null; // waits for one frame
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.WALK;
    }


    void UpdateAnimatedMove()
    {
        if (moveInput != Vector3.zero)
        {
            Move();
            animator.SetFloat("moveX", moveInput.x);
            animator.SetFloat("moveY", moveInput.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void Move()
    {
        rb.MovePosition(transform.position + moveInput * speed * Time.deltaTime);
    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(KnockCoroutine(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockCoroutine(float knockTime)
    {
        if (rb != null)
        {
            yield return new WaitForSeconds(knockTime);
            rb.velocity = Vector2.zero;
            currentState = PlayerState.IDLE;
            rb.velocity = Vector2.zero;
        }
    }
}
