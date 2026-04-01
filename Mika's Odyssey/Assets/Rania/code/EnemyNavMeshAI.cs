using UnityEditor.Build.Content;
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
    private bool isGameOver = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // Ensure enemy is placed on NavMesh
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogError("Enemy not placed on NavMesh");
        }
    }

    private void Update()
    {
        // STOP ALL LOGIC if game over
        if (isGameOver)
            return;

        if (IsCloseEnoughToPlayer())
        {
            FollowPlayer();
        }
        else
        {
            Roam();
        }
    }

    private bool IsCloseEnoughToPlayer()
    {
        return Vector3.Distance(transform.position, player.position) <= closenessThresholdPlayer;
    }

    private void FollowPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    private void Roam()
    {
        agent.isStopped = false;

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

    private void StopMoving()
    {
        isGameOver = true;

        agent.isStopped = true;
        agent.ResetPath(); // VERY IMPORTANT
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player && !isGameOver)
        {
            StopMoving();
            gameManager.GameOver();
        }
    }
}