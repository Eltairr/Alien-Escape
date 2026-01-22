using System.Collections; // Coroutines library
using UnityEngine; 

public class PlayerController : MonoBehaviour 
{
    public float playerSpeed; 
    public GameObject bulletPrefab; 
    public Transform firePoint; 
    public float fireRate = 0.5f; 

    public AudioClip shootSound; 
    [Range(0f, 1f)] public float shootVolume = 0.5f; //[] attribute for slider limits

    private Rigidbody2D rb; 
    private Vector2 leftStickInput; 
    private Vector2 rightStickInput; 

    private Camera mainCamera; 
    private bool canShoot = true; 
    private bool isAlive = true; 

    private HealthSystem healthSystem; 
    private ScreenManager screenManager; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        mainCamera = Camera.main; 
        healthSystem = GetComponent<HealthSystem>(); 
        screenManager = FindObjectOfType<ScreenManager>(); 
        //everytime player is hit, call the freeze function
        if (healthSystem != null)
        {
            healthSystem.OnHealthDepleted.AddListener(() => FreezePlayer(false)); 
        }
    }

    void Update()
    {
        if (!isAlive) return; 

        GetPlayerInput(); 

        //controller and mouse trigger
        if ((IsTriggerPressed() || Input.GetButton("Fire1")) && canShoot)
        {
            Shoot(); 
        }
    }
     
    // movement input
    private void GetPlayerInput()
    {
        leftStickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //unity covers left stick for us
        rightStickInput = new Vector2(Input.GetAxis("R_Horizontal"), Input.GetAxis("R_Vertical"));
    }

    
    private void FixedUpdate()
    {
        if (!isAlive) return; 

        // movement
        Vector2 curMovement = leftStickInput * playerSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + curMovement);

        // Rotation
        if (rightStickInput.magnitude > 0f)
        {
            RotateTowardsJoystick();
        }
        else if (Input.mousePosition != Vector3.zero)
        {
            RotateTowardsMouse();
        }
    }

    private void RotateTowardsJoystick()
    {
        
        float angle = Mathf.Atan2(rightStickInput.y, rightStickInput.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f; // unity right = default
    }

    private void RotateTowardsMouse()
    {
        
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; 
        Vector2 direction = (mousePosition - transform.position).normalized; //vector length = 1
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        rb.rotation = angle - 90f; 
    }

    private void Shoot()
    {
        
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        
        GameObject soundObject = new GameObject("BulletSound"); //assigns a new game object to variable => plays even when destroyed
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = shootSound;
        audioSource.volume = shootVolume;
        audioSource.Play();

        Destroy(soundObject, shootSound.length); 

        canShoot = false; 
        StartCoroutine(ShootingCooldown()); 
    }

    private IEnumerator ShootingCooldown()
    {
        yield return new WaitForSeconds(fireRate); 
        canShoot = true; 
    }

    private bool IsTriggerPressed()
    {
        return Input.GetAxis("RT") > 0.1f; 
    }

    //freeze for both complete and dead, default freeze = not complete
    public void FreezePlayer(bool levelComplete = false)
    {
        isAlive = false; 

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; 
            rb.angularVelocity = 0f; 
            rb.isKinematic = true; //physics
        }

        if (levelComplete)
        {
            if (screenManager != null)
            {
                screenManager.ShowLevelCompleteScreen(); 
            }
        }
        else
        {
            DestroyAllZombies(); 
            if (screenManager != null)
            {
                screenManager.ShowDeathScreen(); 
            }
        }
    }

    //destroy each element in the list
    private void DestroyAllZombies()
    {
        
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Mob");

        foreach (GameObject zombie in zombies) 
        {
            Destroy(zombie);
        }
    }
}
