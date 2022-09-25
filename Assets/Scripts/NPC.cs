using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCState
{
    WALK,
    IDLE
}

public class NPC : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Animator animator;
    private NPCState currentState;
    private int nextPointIndex;
    public Transform[] patrolPoints;
    public bool interactingWithPlayer;


    // Start is called before the first frame update
    void Start()
    {
        interactingWithPlayer = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nextPointIndex = 0;

        if (patrolPoints.Length == 0)
        {
            currentState = NPCState.IDLE;
        }
        else
        {
            currentState = NPCState.WALK;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!interactingWithPlayer && currentState == NPCState.WALK)
        {
            animator.SetBool("moving", true);
            MoveToNextPoint();
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveToNextPoint()
    {
        Vector3 nextPoint = Vector3.MoveTowards(transform.position, patrolPoints[nextPointIndex].position, speed * Time.deltaTime);
        Vector3 directionVector = nextPoint - transform.position;
        ChangeAnim(directionVector);
        rb.MovePosition(nextPoint);

        if (transform.position == patrolPoints[nextPointIndex].position)
        {
            // Change index to next patrol point using modulo (%) since we want to loop around the route
            // eg. if there were 4 patrol points, the index change will be 0 -> 1 -> 2 -> 3 -> 0 -> 1 ....
            int newIndex = (nextPointIndex + 1) % patrolPoints.Length;
            nextPointIndex = newIndex;
        }
    }

    void ChangeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }
    }

    void SetAnimFloat(Vector2 setVector)
    {
        animator.SetFloat("moveX", setVector.x);
        animator.SetFloat("moveY", setVector.y);
    }

}
