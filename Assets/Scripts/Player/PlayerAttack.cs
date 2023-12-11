using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PlayerAttack: OnTriggerEnter called");

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("PlayerAttack: Enemy detected");
            EnemyAI enemyAI = other.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(attackDamage);
            }
        }
    }
}
