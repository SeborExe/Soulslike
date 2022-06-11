using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] EnemyStats enemyStats;
    EnemyBossManager enemyBossManager;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyStats = GetComponentInParent<EnemyStats>();
        enemyBossManager = GetComponentInParent<EnemyBossManager>();
    }
    
    public override void TakeCriticalDamageEvent()
    {
        enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
        enemyManager.pendingCriticalDamage = 0;
    }

    public void CanRotate()
    {
        anim.SetBool("canRotate", true);
    }

    public void StopRotation()
    {
        anim.SetBool("canRotate", false);
    }

    public void EnableCombo()
    {
        anim.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        anim.SetBool("canDoCombo", false);
    }

    public void EnableIsInvulnerable()
    {
        anim.SetBool("isInvulnerable", true);
    }

    public void DisableIsInbulnerable()
    {
        anim.SetBool("isInvulnerable", false);
    }

    public void EnableIsParrying()
    {
        enemyManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        enemyManager.isParrying = false;
    }

    public void EnableCanBeReposted()
    {
        enemyManager.canBeReposted = true;
    }

    public void DisableCanBeReposed()
    {
        enemyManager.canBeReposted = false;
    }

    public void AwardSoulsOnDeath()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        SoulCount soulsCout = FindObjectOfType<SoulCount>();

        if (playerStats != null)
        {
            playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);

            if (soulsCout != null)
            {
                soulsCout.SetSoulsCountText(playerStats.soulCount);
            }
        }
    }

    public void InstantiateParticleBossFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
        GameObject phaseFX = Instantiate(enemyBossManager.particleFX, bossFXTransform.transform);
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidbody.velocity = velocity * enemyManager.moveSpeed;

        if (enemyManager.isRotatingWithRootMotion)
        {
            enemyManager.transform.rotation *= anim.deltaRotation;
        }
    }
}
