using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float stopDistance = 2f;

    private NavMeshAgent agent;
    
    public float damageRate = 1f; // saniyede bir saldırı
    public int damageAmount = 10;

    private float nextAttackTime = 0f;

    public EnemySoundManager EnemySoundManager;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on " + gameObject.name);
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log("Player found and assigned by tag.");
            }
            else
            {
                Debug.LogError("Player object not found in the scene! Make sure it has the 'Player' tag.");
            }
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError(gameObject.name + " is not placed on a NavMesh!");
        }
        
    }

    void Update()
    {
        if (player == null || agent == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        Debug.DrawLine(transform.position, player.position, Color.red); // Görsel debug

        if (distance > stopDistance)
        {
            agent.SetDestination(player.position);
            // Debug.Log(gameObject.name + " is chasing player.");
        }
        else
        {
            agent.ResetPath();
            // Debug.Log(gameObject.name + " reached player, stopping.");

            // Düşman oyuncuya ulaştığında hasar verme
            if (Time.time >= nextAttackTime)
            {
                PlayerHealth ph = player.GetComponent<PlayerHealth>();
                if (ph != null)
                {
                    // Debug.Log(gameObject.name + " ATTACKING player, damage: " + damageAmount);
 EnemySoundManager.PlayAttackSound();
                    ph.TakeDamage(damageAmount);
                   
                    nextAttackTime = Time.time + damageRate;
                }
                else
                {
                    Debug.LogError("PlayerHealth component not found on player!");
                }
            }
        }
    }
}
