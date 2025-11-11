using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;

    // 1 = facing right, -1 = facing left
    private int facingDirection = 1;

    // private bool isChasing;

    public float attackRange = 1.2f;
    private EnemyState enemyState;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;

    // Use FixedUpdate for physics-based movement
    void FixedUpdate()
    {
        if (enemyState == EnemyState.Idle || player == null)
        {
            // not chasing: ensure we don't keep moving
            if (rb != null) rb.velocity = Vector2.zero;
            return;
        }

        else if (enemyState == EnemyState.Chasing)
        {
            Chaise();
        }

        else if (enemyState == EnemyState.Attacking)
        {
            rb.velocity = Vector2.zero;
        }
    }
    
    void Chaise()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            ChangeState(EnemyState.Attacking);
        }

        // Flip if player is on the opposite side
        else if ((player.position.x > transform.position.x && facingDirection == -1) ||
            (player.position.x < transform.position.x && facingDirection == 1))
        {
            Flip();
        }

        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        if (rb != null)
        {
            rb.velocity = direction * speed;
            // Also flip based on actual movement velocity so the sprite faces the direction it moves
            float vx = rb.velocity.x;
            if (vx > 0f && facingDirection == -1)
            {
                Flip();
            }
            else if (vx < 0f && facingDirection == 1)
            {
                Flip();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player == null)
            {
                player = collision.transform;
            }
            ChangeState(EnemyState.Chasing);
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }
    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            if (rb != null) rb.velocity = Vector2.zero;
            player = null;
            ChangeState(EnemyState.Idle);
        }
    }

    private void Start()
    {
        // ensure rb is assigned and facingDirection matches the initial localScale
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        facingDirection = transform.localScale.x > 0f ? 1 : -1;
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    void ChangeState(EnemyState newState)
    {
        // Exit current animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", false);

        // Update our current state
        enemyState = newState;

        // Enter new animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true);
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
}