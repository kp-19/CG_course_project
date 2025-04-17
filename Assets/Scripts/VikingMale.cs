using UnityEngine;

public class VikingMale : MonoBehaviour
{
    private Animator animator;
    public int hp = 100;
    private bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get animator from child object
        animator = GetComponentInChildren<Animator>();
        
        if (animator == null)
        {
            Debug.LogWarning("Animator not found in children of " + gameObject.name);
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
                animator.SetBool("attack", true);
            }
            TakeDamage(10);
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
