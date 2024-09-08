using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyNavMeshAgent : MonoBehaviour
{
    [SerializeField] Enemy self;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float enemyDetectionRadius;
    PlayerController player;
    EnemyNavMeshAgent[] nearbyEnemies;
    [SerializeField] float updateNearbyEnemiesTime;
    [SerializeField] Vector3 overlapSphereOffset;
    [SerializeField] int defaultPriority = 0;
    public bool CanMove { get; private set; }
    void Start()
    {
        player = PlayerController.instance;
        StartCoroutine(CheckNearbyEnemies());
        CanMove = true;
    }

    public void MoveTowardsPlayer()
    {
        agent.SetDestination(player.transform.position);
    }
    public void SetEnabled(bool value)
    {
        agent.isStopped = !value;
    }

    EnemyNavMeshAgent[] GetEnemiesInRange()
    {
        Vector3 origin = transform.position + overlapSphereOffset;
        return Physics.OverlapSphere(origin, enemyDetectionRadius, enemyLayer).Where(n =>
        n.name != self.name &&
        !n.name.Contains("Hit") &&
        !n.name.Contains("Hurt")).ToArray().Select(n => n.GetComponentInChildren<EnemyNavMeshAgent>())
            .ToArray();
    }

    IEnumerator CheckNearbyEnemies()
    {
        nearbyEnemies = GetEnemiesInRange();
        CanMove = true;
        if (nearbyEnemies.Any())
        {
            foreach (var navAgent in nearbyEnemies)
            {
                if (Vector3.Distance(navAgent.transform.position, player.transform.position) <
                    Vector3.Distance(transform.position, player.transform.position) &&
                    navAgent.agent.avoidancePriority > agent.avoidancePriority)
                {
                    int prioDiff = navAgent.agent.avoidancePriority - agent.avoidancePriority;
                    agent.avoidancePriority = Mathf.Clamp(
                        defaultPriority + prioDiff + Random.Range(1, 50),
                        0,
                        100);
                }
            }
        }
        else
        {
            agent.avoidancePriority = defaultPriority;
        }
        yield return new WaitForSeconds(updateNearbyEnemiesTime);
        StartCoroutine(CheckNearbyEnemies());
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + overlapSphereOffset;
        Gizmos.DrawWireSphere(origin, enemyDetectionRadius);
    }
    public float GetDistRemaining()
    {
        return agent.remainingDistance;
    }
}
