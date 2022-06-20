using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    protected Collider damageCollider;
    public CharacterManager characterManager;
    public bool enabledDamageColliderOnStartUp = false;

    [Header("Team ID")]
    public int teamIDNumber = 0;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Damage")]
    public int physicalDamage = 25;
    public int fireDamage;
    public int magicDamage;

    protected virtual void Awake()
    {
        damageCollider = GetComponent<Collider>();
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
        if (collision.tag == "Character")
        {
            CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
            CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
            CharacterEffectsManager enemyEffects = collision.GetComponent<CharacterEffectsManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            if (enemyManager != null)
            {
                if (enemyStats.teamIDNumber == teamIDNumber) return;

                if (enemyManager.isParrying)
                {
                    //Check if you are parryable
                    characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
                else if (shield != null && enemyManager.isBlocking)
                {
                    float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                    float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorption) / 100;

                    if (enemyStats != null)
                    {
                        enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), Mathf.RoundToInt(fireDamageAfterBlock), "Block_Guard");
                        return;
                    }
                }
            }

            if (enemyStats != null)
            {
                if (enemyStats.teamIDNumber == teamIDNumber) return;

                enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                enemyStats.totalPoiseDefense -= poiseBreak;

                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                enemyEffects.PlayBloodSplatterFX(contactPoint);

                if (enemyStats.totalPoiseDefense > poiseBreak)
                {
                    enemyStats.TakeDamageNoAnimation(physicalDamage, 0);
                }
                else
                {
                    enemyStats.TakeDamage(physicalDamage, 0);
                }
            }
        }
        
        if (collision.tag == "Illusionary Wall")
        {
            IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();
            illusionaryWall.wallHasBeenHit = true;
        }
    }
}

