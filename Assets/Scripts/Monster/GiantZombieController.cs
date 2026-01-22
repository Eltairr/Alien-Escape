using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GiantZombieController : MonoBehaviour
{
    [Header("References")]
    private HealthSystem _healthSystem;
    private bool isDead = false;

    [Header("Health Settings")]
    public float damageTakenPerHit = 1f;
    public GameObject[] bloodSplats;
    public Transform bloodSpawnPoint;

    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float attackCooldown = 1f;
    private bool canAttack = true;

    [Header("Audio Settings")]
    public AudioClip attackSound;
    [Range(0f, 1f)] public float attackVolume = 0.5f;
    public AudioClip deathSound;
    [Range(0f, 1f)] public float deathVolume = 0.5f;

    private ZombieAnimation zombieAnimation;

    [Header("Health Bar")]
    public GameObject healthBarPrefab;
    private Slider healthBarSlider;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        zombieAnimation = GetComponent<ZombieAnimation>();

        if (_healthSystem != null)
        {
            _healthSystem.OnHealthDepleted.AddListener(HandleDeath);
        }

        // Find health bar if present
        healthBarSlider = GetComponentInChildren<Slider>();
        if (healthBarSlider != null)
        {
            healthBarSlider.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (healthBarSlider != null && healthBarSlider.gameObject.activeSelf)
        {
            healthBarSlider.transform.position = transform.position + Vector3.up * 2f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            _healthSystem.ApplyDamage(damageTakenPerHit);
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
            healthBarSlider.value = _healthSystem.HealthPercentage;
        }
    }

    private void HandleDeath()
    {
        if (isDead) return;
        isDead = true;

        PlayDeathSound();
        SpawnBloodSplat();

        if (healthBarSlider != null)
        {
            Destroy(healthBarSlider.gameObject);
        }

        Destroy(gameObject);
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
                if (zombieAnimation != null)
                {
                    zombieAnimation.StartAttackAnimation();
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && zombieAnimation != null)
        {
            zombieAnimation.StopAttackAnimation();
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void PlayAttackSound()
    {
        if (attackSound == null) return;
        
        GameObject soundObject = new GameObject("ZombieAttackSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = attackSound;
        audioSource.volume = attackVolume;
        audioSource.Play();
        
        Destroy(soundObject, attackSound.length);
    }

    private void PlayDeathSound()
    {
        if (deathSound == null) return;
        
        GameObject soundObject = new GameObject("ZombieDeathSound");
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
        Instantiate(bloodSplats[randomIndex], bloodSpawnPoint.position, Quaternion.identity);
    }
}
