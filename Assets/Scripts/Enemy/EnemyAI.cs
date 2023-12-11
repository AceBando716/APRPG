using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject healthBarPrefab;
    private GameObject healthBar;
    private RectTransform healthBarForeground;
    public int maxHealth = 100;

    private int currentHealth;
    float chaseSpeed = 10f;
    float wanderSpeed = 5f;
    float sightRange = 20f;
    float attackRange = 10f; // Define attack range

    Vector3 wanderTarget;
    float wanderRange = 20.0f; // Define wander range

    GameObject player; 
    NavMeshAgent agent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        wanderTarget = GetRandomWanderTarget();

        currentHealth = maxHealth;

        healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        healthBar.transform.SetParent(gameObject.transform);

        // Find the healthBarForeground inside healthBarBackground
        Transform healthBarBackground = healthBar.transform.Find("healthBarBackground");
        if (healthBarBackground != null)
        {
            healthBarForeground = healthBarBackground.Find("healthBarForeground").GetComponent<RectTransform>();
            if (healthBarForeground == null)
            {
                Debug.LogError("HealthBarForeground not found in healthBarPrefab.");
            }
        }
        else
        {
            Debug.LogError("healthBarBackground not found in healthBarPrefab.");
        }
    }

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

    void LateUpdate()
    {
        if (healthBar != null && Camera.main != null)
        {
            healthBar.transform.LookAt(Camera.main.transform);
        }
    }

    bool IsPlayerInSightRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < sightRange;
    }

    bool IsPlayerInAttackRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < attackRange;
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
        return new Vector3(randomX, 0, randomZ);
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            Debug.Log("Enemy died!");
        }
        else
        {
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarForeground != null)
        {
            float healthPercentage = (float)currentHealth / maxHealth;
            Debug.Log($"Updating health bar: {healthPercentage}");
            healthBarForeground.localScale = new Vector3(healthPercentage, 1, 1);
        }
    }

    public void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}





