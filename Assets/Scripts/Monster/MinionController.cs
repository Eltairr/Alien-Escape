using UnityEngine;

public class MinionController : MonoBehaviour
{
    [Header("Health Settings")]
    public float damageTakenPerHit = 1f;
    private HealthSystem _healthSystem;

    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float attackCooldown = 1f;
    private bool canAttack = true;

    [Header("Blood Splats")]
    public GameObject[] bloodSplats;
    public Transform bloodSpawnPoint;

    [Header("Minion Sounds")]
    public AudioClip attackSound;
    [Range(0f, 1f)] public float attackVolume = 0.5f;

    public AudioClip deathSound;
    [Range(0f, 1f)] public float deathVolume = 0.5f;

    private ZombieAnimation minionAnimation;
    private bool isDead = false; // Prevents multiple death triggers

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        minionAnimation = GetComponent<ZombieAnimation>();

        if (_healthSystem != null)
        {
            _healthSystem.OnHealthDepleted.AddListener(HandleDeath);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            _healthSystem.ApplyDamage(damageTakenPerHit);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            var playerHealth = collision.gameObject.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.ApplyDamage(attackDamage);
                StartCoroutine(AttackCooldown());

                PlayAttackSound();
                if (minionAnimation != null)
                {
                    minionAnimation.StartAttackAnimation();
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (minionAnimation != null)
            {
                minionAnimation.StopAttackAnimation();
            }
        }
    }

    private System.Collections.IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void HandleDeath()
    {
        if (isDead) return; // Prevent multiple calls
        isDead = true;

        PlayDeathSound();
        SpawnBloodSplat();
        Destroy(gameObject);
    }

    private void PlayAttackSound()
    {
        if (attackSound == null) return;

        GameObject soundObject = new GameObject("MinionAttackSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = attackSound;
        audioSource.volume = attackVolume;
        audioSource.Play();

        Destroy(soundObject, attackSound.length);
    }

    private void PlayDeathSound()
    {
        if (deathSound == null) return;

        GameObject soundObject = new GameObject("MinionDeathSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = deathSound;
        audioSource.volume = deathVolume;
        audioSource.Play();

        Destroy(soundObject, deathSound.length);
    }

    private void SpawnBloodSplat()
    {
        if (bloodSplats.Length == 0) return;

        int randomIndex = Random.Range(0, bloodSplats.Length);
        GameObject splat = Instantiate(bloodSplats[randomIndex], bloodSpawnPoint.position, Quaternion.identity);
    }
}
