using System.Collections;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;
    private PlayerDetection playerDetection;
    private HealthSystem _healthSystem;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;

    [Header("Audio Settings")]
    public AudioClip gunshotSound;
    public AudioClip deathSound;
    public AudioClip bulletHitSound;
    [Range(0f, 1f)] public float gunshotVolume = 1f;
    [Range(0f, 1f)] public float deathVolume = 1f;
    [Range(0f, 1f)] public float bulletHitVolume = 1f;

    [Header("Blood Splats")]
    public GameObject[] bloodSplats;
    public Transform bloodSpawnPoint;

    private bool isShooting = false;
    private Transform player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDetection = GetComponent<PlayerDetection>();
        _healthSystem = GetComponent<HealthSystem>();

        if (_healthSystem != null)
        {
            _healthSystem.OnHealthDepleted.AddListener(HandleDeath);
        }
    }

    private void Update()
    {
        if (playerDetection.IsPlayerVisible)
        {
            player = GameObject.FindWithTag("Player").transform;
            HandleCombat();
        }
    }

    private void HandleCombat()
    {
        if (player == null) return;

        RotateFirePointToPlayer();

        if (!isShooting)
        {
            StartCoroutine(ShootLoop());
        }
    }

    private void RotateFirePointToPlayer()
    {
        if (player == null || firePoint == null) return;

        Vector2 direction = (player.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        firePoint.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private IEnumerator ShootLoop()
    {
        isShooting = true;

        while (playerDetection.IsPlayerVisible)
        {
            ShootAtPlayer();
            yield return new WaitForSeconds(fireRate);
        }

        isShooting = false;
    }

    private void ShootAtPlayer()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            PlayGunshotSound();
        }
    }

    private void PlayGunshotSound()
    {
        if (gunshotSound != null)
        {
            PlaySoundAtPosition(gunshotSound, gunshotVolume);
        }
    }

    private void HandleDeath()
    {
        PlayDeathSound();
        SpawnBloodSplat();
        Destroy(gameObject);
    }

    private void PlayDeathSound()
    {
        if (deathSound != null)
        {
            PlaySoundAtPosition(deathSound, deathVolume);
        }
    }

    private void PlayBulletHitSound()
    {
        if (bulletHitSound != null)
        {
            PlaySoundAtPosition(bulletHitSound, bulletHitVolume);
        }
    }

    private void SpawnBloodSplat()
    {
        if (bloodSplats.Length == 0) return;

        int randomIndex = Random.Range(0, bloodSplats.Length);
        Instantiate(bloodSplats[randomIndex], bloodSpawnPoint.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            _healthSystem.ApplyDamage(1f);
            PlayBulletHitSound();
        }
    }

    private void PlaySoundAtPosition(AudioClip clip, float volume)
    {
        GameObject soundObject = new GameObject("ShooterSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(soundObject, clip.length);
    }
}
