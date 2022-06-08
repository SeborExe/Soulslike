using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    BoxCollider damageCollider;
    public CharacterManager characterManager;
    public bool enabledDamageColliderOnStartUp = false;

    public int currentWeaponDamage = 25;

    private void Awake()
    {
        damageCollider = GetComponent<BoxCollider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = enabledDamageColliderOnStartUp;
    }

    public void EnabelDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            if (enemyCharacterManager != null)
            {
                if (enemyCharacterManager.isParrying)
                {
                    //Check if you are parryable
                    characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
                else if (shield != null && enemyCharacterManager.isBlocking)
                {
                    float physicalDamageAfterBlock = currentWeaponDamage -
                        (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                    if (playerStats != null)
                    {
                        playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block_Guard");
                        return;
                    }
                }
            }

            if (playerStats != null)
            {
                playerStats.TakeDamage(currentWeaponDamage);
            }
        }

        if (collision.tag == "Enemy")
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            if (enemyCharacterManager != null)
            {
                if (enemyCharacterManager.isParrying)
                {
                    //Check if you are parryable
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }

                else if (shield != null && enemyCharacterManager.isBlocking)
                {
                    float physicalDamageAfterBlock = currentWeaponDamage -
                        (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                    if (enemyStats != null)
                    {
                        enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block_Guard");
                        return;
                    }
                }
            }

            if (enemyStats != null)
            {
                enemyStats.TakeDamage(currentWeaponDamage);
            }
        }
        
        if (collision.tag == "Illusionary Wall")
        {
            IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();
            illusionaryWall.wallHasBeenHit = true;
        }
    }
}

