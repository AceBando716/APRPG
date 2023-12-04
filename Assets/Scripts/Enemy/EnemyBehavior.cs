using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
 

public class EnemyBehavior : MonoBehaviour
{
    public Transform target;
    public EnemyReferences enemyReferences;

    private float pathUpdateDeadline;
    private float shootingDistance;

    private void Awake() {
        enemyReferences = GetComponent<EnemyReferences>();
        shootingDistance = enemyReferences.navMeshAgent.stoppingDistance;
        pathUpdateDeadline = 0f; // Initialize pathUpdateDeadline
    }
    
    void Update() {
        if (target != null) {
            bool inRange = Vector3.Distance(transform.position, target.position) <= shootingDistance;

            if (inRange) {
                LookAtTarget();
            }
            else {
                UpdatePath();
            }
        }
    }

    private void LookAtTarget() {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    private void UpdatePath() {
        if (Time.time >= pathUpdateDeadline) {
            Debug.Log("Updating Path"); // Fixed Debug.Log typo
            pathUpdateDeadline = Time.time + enemyReferences.pathUpdateDelay;
            enemyReferences.navMeshAgent.SetDestination(target.position);
        }
    }
}

