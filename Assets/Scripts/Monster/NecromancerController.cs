using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NecromancerController : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerDetection playerDetection;
    private HealthSystem healthSystem;
    private bool isDead = false; // Prevent multiple death triggers

    [Header("Health Settings")]
    public float damageTakenPerHit = 1f;
    public GameObject[] bloodSplats;
    public Transform bloodSpawnPoint;

    [Header("Audio Settings")]
    public AudioClip deathSound;
    [Range(0f, 1f)] public float deathVolume = 0.5f;
    private AudioSource audioSource;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float moveChangeInterval = 3f;
    private Vector2 moveDirection;
    private bool isMoving = false;

    [Header("Attack Settings")]
    public GameObject minionPrefab;
    public Transform[] spawnPoints;
    public float attackCooldown = 5f;
    private bool canAttack = true;

    [Header("Health Bar")]
    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;
    private Slider healthBarSlider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerDetection = GetComponent<PlayerDetection>();
        healthSystem = GetComponent<HealthSystem>();
        audioSource = GetComponent<AudioSource>();

        if (healthSystem != null)
        {
            healthSystem.OnHealthDepleted.AddListener(HandleDeath);
        }

        healthBarSlider = GetComponentInChildren<Slider>();

        if (healthBarSlider != null)
        {
            healthBarSlider.gameObject.SetActive(false); // Initially hidden
        }
    }

    private void Start()
    {
        StartCoroutine(ChangeMovementRoutine());
    }

    private void Update()
    {
        if (playerDetection.IsPlayerVisible)
        {
            HandleCombat();
        }
    }

    private void FixedUpdate()
    {
        MoveBoss();
    }

    private void MoveBoss()
    {
        if (moveDirection != Vector2.zero)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
            SetAnimation(moveDirection);
            isMoving = true;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            SetIdleAnimation();
            isMoving = false;
        }
    }

    private IEnumerator ChangeMovementRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveChangeInterval);
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0: moveDirection = Vector2.up; break;
            case 1: moveDirection = Vector2.down; break;
            case 2: moveDirection = Vector2.left; break;
            case 3: moveDirection = Vector2.right; break;
        }
        isMoving = true;
    }

    private void HandleCombat()
    {
        if (canAttack && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            StartCoroutine(AttackSequence());
        }
    }

    private IEnumerator AttackSequence()
    {
        canAttack = false;
        rb.linearVelocity = Vector2.zero;
        SetIdleAnimation();

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        SpawnMinions();

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void SpawnMinions()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            Vector2 spawnPosition = spawnPoint.position;
            Collider2D obstacle = Physics2D.OverlapCircle(spawnPosition, 0.5f, LayerMask.GetMask("Obstacle"));
            if (obstacle != null)
            {
                spawnPosition = transform.position; // Spawn at boss if blocked
            }

            Instantiate(minionPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void SetAnimation(Vector2 direction)
    {
        if (animator == null) return;

        animator.ResetTrigger("MoveUp");
        animator.ResetTrigger("MoveDown");
        animator.ResetTrigger("MoveLeft");
        animator.ResetTrigger("MoveRight");
        animator.ResetTrigger("Idle");

        if (direction == Vector2.up) animator.SetTrigger("MoveUp");
        else if (direction == Vector2.down) animator.SetTrigger("MoveDown");
        else if (direction == Vector2.left) animator.SetTrigger("MoveLeft");
        else if (direction == Vector2.right) animator.SetTrigger("MoveRight");
    }

    private void SetIdleAnimation()
    {
        if (animator == null) return;

        animator.ResetTrigger("MoveUp");
        animator.ResetTrigger("MoveDown");
        animator.ResetTrigger("MoveLeft");
        animator.ResetTrigger("MoveRight");
        animator.ResetTrigger("Attack");

        animator.SetTrigger("Idle");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            rb.linearVelocity = Vector2.zero;
            moveDirection = Vector2.zero;
            SetIdleAnimation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            healthSystem.ApplyDamage(damageTakenPerHit);
            ShowHealthBar();
        }
    }

    private void ShowHealthBar()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.gameObject.SetActive(true);
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = healthSystem.HealthPercentage;
        }
    }


private void SpawnBloodSplat()
{
    if (bloodSplats.Length == 0) return;

    // first bloodpslat spawn
    int randomIndex1 = Random.Range(0, bloodSplats.Length);
    GameObject splat1 = Instantiate(bloodSplats[randomIndex1], bloodSpawnPoint.position, Quaternion.identity);
    splat1.transform.localScale = new Vector3(1.2f, 1.2f, 1); // Slight size difference for variation

    // alightly offset the second bloodsplat
    int randomIndex2 = Random.Range(0, bloodSplats.Length);
    Vector2 offset = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
    GameObject splat2 = Instantiate(bloodSplats[randomIndex2], bloodSpawnPoint.position + (Vector3)offset, Quaternion.identity);
    splat2.transform.localScale = new Vector3(1f, 1f, 1); // Slightly smaller size

}


    private void HandleDeath()
    {
        if (isDead) return;
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Death");
        SpawnBloodSplat();
        PlayDeathSound();
        DestroyAllMinions();

        if (healthBarSlider != null)
        {
            Destroy(healthBarSlider.gameObject);
        }

        StartCoroutine(DestroyAfterAnimation(animator.GetCurrentAnimatorStateInfo(0).length));
    }

    private void DestroyAllMinions()
    {
        GameObject[] minions = GameObject.FindGameObjectsWithTag("Minion");

        foreach (GameObject minion in minions)
        {
            Destroy(minion);
        }
    }

    private void PlayDeathSound()
    {
        if (deathSound == null) return;

        GameObject soundObject = new GameObject("NecromancerDeathSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = deathSound;
        audioSource.volume = deathVolume;
        audioSource.Play();

        Destroy(soundObject, deathSound.length);
    }

    private IEnumerator DestroyAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
