using UnityEngine; 

public class BulletController : MonoBehaviour 
{
    public float speed; 
    public float lifetime; 
    
    public GameObject bulletImpactPrefab; 
    public float impactDestroyTime = 0.5f; // set default value/cast as float

    private Vector2 direction; 
    private Transform player; 

    void Awake() 
    {
        direction = transform.up; 
        player = GameObject.FindGameObjectWithTag("Player").transform; // Getting transform position of tag player
    }

    void Start() 
    {
        //colisions before shot
        // storing the result of the raycast into "hit" before using the function Physics2D.Raycast
        RaycastHit2D hit = Physics2D.Raycast(
            player.position, 
            direction, 
            Vector2.Distance(player.position, transform.position), 
            LayerMask.GetMask("Environment") //  makes sure that only colliders with envirtonment tag are detected for raycast
        );
        
        if (hit.collider != null) // refers to the collider that was colided with 
        {
            Destroy(gameObject); 
            return; 
        }

        Invoke(nameof(DestroyBullet), lifetime); //delay
    }

    void Update() 
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime; // move bullet each frame indipendently 
    }

//collisions after shot
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (bulletImpactPrefab != null) 
        {
            GameObject impact = Instantiate(
                bulletImpactPrefab, 
                transform.position, 
                Quaternion.identity 
            );
            Destroy(impact, impactDestroyTime); 
        }
        
        Destroy(gameObject); 
    }

    private void DestroyBullet() 
    {
        Destroy(gameObject); 
    }
}
