using UnityEngine;

public class MobDeath : MonoBehaviour
{
    private HealthSystem _healthSystem;
    private MobStatusUpdater _mobStatusUpdater;
    private bool isDead = false;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _mobStatusUpdater = FindObjectOfType<MobStatusUpdater>();

        if (_healthSystem != null)
        {
            _healthSystem.OnHealthDepleted.AddListener(OnMobDeath);
        }
        else
        {
            Debug.LogWarning("no health system found");
        }
    }

    public void OnMobDeath()
    {
        if (isDead) return;
        isDead = true;

    
        if (!gameObject.CompareTag("Minion")) // Only count real mobs
        {
            if (_mobStatusUpdater != null)
            {
                _mobStatusUpdater.MobKilled();
            }
        }

        Destroy(gameObject);
    }
}
