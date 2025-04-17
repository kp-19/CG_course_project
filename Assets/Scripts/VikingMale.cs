using UnityEngine;

public class VikingMale : MonoBehaviour
{
    private Animator animator;
    public int hp = 100;
    private bool isDead = false;
    private Transform playerTransform;
    private float lastAttackTime = 0f;
    private float attackCooldown = 1.01f; // Adjust based on your animation length

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get animator from child object
        animator = GetComponentInChildren<Animator>();
        
        if (animator == null)
        {
            Debug.LogWarning("Animator not found in children of " + gameObject.name);
        }
        
        // Find the player object
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (animator != null)
            {
                animator.SetBool("run", false);
                animator.SetBool("attack", true);
                
                // Check if enough time has passed since last attack
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    DamagePlayer();
                    lastAttackTime = Time.time;
                }
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (animator != null)
            {
                animator.SetBool("attack", false);
            }
        }
    }

    // Called by animation event or on a timer
    public void DamagePlayer()
    {
        if (isDead) return;
        
        // Find player and deal damage
        if (playerTransform != null)
        {
            PlayerUIController playerUI = playerTransform.GetComponent<PlayerUIController>();
            if (playerUI != null)
            {
                // Deal 10 damage to player
                playerUI.SetHealth(playerUI.health - 10);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        
        hp -= damage;
        
        if (hp <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        isDead = true;
        hp = 0;
        
        if (animator != null)
        {
            animator.SetBool("died", true);
        }

        // Disable the EnemyFollow component when dead
        EnemyFollow enemyFollow = GetComponent<EnemyFollow>();
        if (enemyFollow != null)
        {
            enemyFollow.enabled = false;
        }
        
        Invoke("DestroySelf", 10f);
    }

    private void DestroySelf()
    {
        GameObject.Destroy(gameObject);
    }
}
