using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPathFinder : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameManager gameManager;

    [Header("Behavior")]
    [SerializeField] private float closenessThresholdPlayer = 60f;
    [SerializeField] private float roamingRadius = 20f;
    [SerializeField] private float timeBetweenRoamTargets = 5f;

    private NavMeshAgent agent;
    private float elapsedTime;

    private bool isDead = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogError("Enemy not placed on NavMesh");
        }
    }

    private bool dead;

private void Update()
{
    if (dead) return;

    agent.SetDestination(player.position);
}

private void OnTriggerEnter(Collider other)
{
    if (other.transform == player)
    {
        dead = true;
        agent.ResetPath();
        agent.isStopped = true;
    }
}

    private bool IsCloseEnoughToPlayer()
    {
        return Vector3.Distance(transform.position, player.position) <= closenessThresholdPlayer;
    }

    private void FollowPlayer()
    {
        // No need to constantly toggle isStopped
        agent.SetDestination(player.position);
    }

    private void Roam()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= timeBetweenRoamTargets || !agent.hasPath)
        {
            Vector3 randomPoint = GetRandomPoint(transform.position, roamingRadius);
            agent.SetDestination(randomPoint);
            elapsedTime = 0f;
        }
    }

    private Vector3 GetRandomPoint(Vector3 origin, float radius)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += origin;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return origin;
    }

    private void StopEnemy()
    {
        isDead = true;

        agent.ResetPath();
        agent.isStopped = true;
    }

}