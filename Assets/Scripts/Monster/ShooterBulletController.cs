using UnityEngine;

public class ShooterBulletController : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public float damageAmount = 1f;

    public GameObject bulletImpactPrefab;
    public float impactDestroyTime = 0.5f;

    private Vector2 direction;

    void Awake()
    {
        direction = transform.up;
    }

    void Start()
    {
        Invoke(nameof(DestroyBullet), lifetime); //bullet lifetime
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // damage the player if hit
        if (other.CompareTag("Player"))
        {
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.ApplyDamage(damageAmount);
            }
        }

        // Create an impact effect
        if (bulletImpactPrefab != null)
        {
            GameObject impact = Instantiate(bulletImpactPrefab, transform.position, Quaternion.identity);
            Destroy(impact, impactDestroyTime);
        }

        Destroy(gameObject); // remove bullet on impact
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
