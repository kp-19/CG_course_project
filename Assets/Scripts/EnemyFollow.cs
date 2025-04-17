using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    private float detectionRadius = 30f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
                } else{
                    Debug.LogWarning("Agent not on NavMesh!");
                    gameObject.SetActive(false);
                    gameObject.SetActive(true);
                }
            }
            else
            {
                if (agent.isOnNavMesh)
                {
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
