using UnityEngine;

public class ZombieAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private bool isAttacking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (animator == null)
        {
            this.enabled = false;
        }
    }

    private void Update()
    {
        HandleMovementAnimation();
    }

    private void HandleMovementAnimation()
    {
        float speed = rb.linearVelocity.magnitude; 
        bool isMoving = speed > 0.1f;

        if (!isAttacking) // prevent movement animation from overriding attack animation
        {
            animator.SetBool("isMoving", isMoving);
        }
    }

    public void StartAttackAnimation()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
    }

    public void StopAttackAnimation()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    public void StopAllAnimations()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", false);
    }

}
