using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerController playerController;

    public int baseAttackDamage = 10;
    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found on the player.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PlayerAttack: OnTriggerEnter called");

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("PlayerAttack: Enemy detected");
            EnemyAI enemyAI = other.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                int attackDamage = Mathf.RoundToInt(baseAttackDamage * playerController.damageMultiplier);

                enemyAI.TakeDamage(attackDamage);
            }
        }
    }
}
