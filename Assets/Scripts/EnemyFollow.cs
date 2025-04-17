using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    private float detectionRadius = 30f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= detectionRadius)
            {
                if (agent.isOnNavMesh)
                {
                    agent.SetDestination(player.position);
                    if (animator != null && animator.GetBool("attack") == false)
                    {
                        animator.SetBool("run", true);
                    }
                } else{
                    gameObject.SetActive(false);
                    gameObject.SetActive(true);
                }
            }
            else
            {
                if (agent.isOnNavMesh)
                {
                    if (animator != null)
                    {
                        animator.SetBool("run", false);
                    }
                    agent.ResetPath();
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
