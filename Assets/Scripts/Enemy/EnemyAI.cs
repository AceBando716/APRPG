using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class EnemyAI : MonoBehaviour
{
    float chaseSpeed = 10f;
    float wanderSpeed = 5f;
    float sightRange = 20f;
    float attackRange = 10f; // Define attack range

    Vector3 wanderTarget;
    float wanderRange = 20.0f; // Define wander range

    GameObject player; // Corrected Gameobject to GameObject
    NavMeshAgent agent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Corrected FindObjectWithTag to FindGameObjectWithTag
        agent = GetComponent<NavMeshAgent>();
        wanderTarget = GetRandomWanderTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerInSightRange() && !IsPlayerInAttackRange())
        {
            ChasePlayer();
        }
        else if (IsPlayerInAttackRange())
        {
            AttackPlayer();
        }
        else
        {
            Wander();
        }
    }

    bool IsPlayerInSightRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < sightRange; // Added missing semicolon
    }

    bool IsPlayerInAttackRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < attackRange; // Corrected typo and added missing semicolon
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.transform.position);
    }

    void AttackPlayer()
    {
        Debug.Log("Attacking Player");
    }

    void Wander()
    {
        if (Vector3.Distance(transform.position, wanderTarget) < 1.0f)
        {
            wanderTarget = GetRandomWanderTarget();
        }
        agent.speed = wanderSpeed;
        agent.SetDestination(wanderTarget);
    }

    Vector3 GetRandomWanderTarget()
    {
        float randomX = Random.Range(-wanderRange, wanderRange);
        float randomZ = Random.Range(-wanderRange, wanderRange);
        return new Vector3(randomX, 0, randomZ); // Assuming the terrain is flat at y = 0
    }
}

