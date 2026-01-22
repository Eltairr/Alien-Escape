using UnityEngine;
using UnityEngine.Events; 

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float _currentHP; 
    [SerializeField] private float _maxHP; 

    public UnityEvent OnHealthDepleted; 
    public UnityEvent OnHealthReduced; 

    public bool Invulnerable { get; set; } 

    private ScreenManager screenManager; 

    private void Start()
    {
        _currentHP = _maxHP; 
        
        
        screenManager = FindObjectOfType<ScreenManager>();
    }

    // Read-only getter property reamining health
    public float HealthPercentage => _currentHP / _maxHP;

    public void ApplyDamage(float damageValue)
    {
        
        if (_currentHP <= 0 || Invulnerable) return;

        _currentHP -= damageValue; 
        _currentHP = Mathf.Max(_currentHP, 0); 

        if (_currentHP == 0)
        {
            
            if (OnHealthDepleted != null)
            {
                OnHealthDepleted.Invoke();
                OnHealthDepleted.RemoveAllListeners(); 
            }

            
            if (gameObject.CompareTag("Player") && screenManager != null)
            {
                screenManager.ShowDeathScreen();
            }
        }
        else
        {
            
            OnHealthReduced.Invoke();
        }
    }

    public void RestoreHealth(float amount)
    {
        
        if (_currentHP >= _maxHP) return;

        _currentHP += amount; 
        _currentHP = Mathf.Min(_currentHP, _maxHP); 
    }

    private void OnDestroy()
    {
        OnHealthDepleted.RemoveAllListeners();
        OnHealthReduced.RemoveAllListeners();
    }
}
