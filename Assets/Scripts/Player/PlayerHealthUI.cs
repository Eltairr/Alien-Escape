using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealthBar : MonoBehaviour
{
    [Header("UI Elements")] 
    [SerializeField]
    private Image healthFillImage; 
    
    [SerializeField]
    private Image healthBackgroundImage; 

    [SerializeField]
    private Color fullHealthColor = Color.green; 
    [SerializeField]
    private Color zeroHealthColor = Color.red;   

    private HealthSystem healthSystem; 

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>(); 
        if (healthSystem == null) 
        {
            Debug.LogError("HealthSystem not found"); 
        }
    }

    private void Start()
    {
        UpdateHealthBar(); 
        
        
        healthSystem.OnHealthReduced.AddListener(UpdateHealthBar); 
        healthSystem.OnHealthDepleted.AddListener(UpdateHealthBar); 
    }

    private void UpdateHealthBar()
    {
        if (healthFillImage != null) // Ensure health bar image exists
        {
            float healthPercentage = healthSystem.HealthPercentage; 
            healthFillImage.fillAmount = healthPercentage; 
            
            //interpolate 0-1
            healthFillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, healthPercentage);
        }
    }
}
