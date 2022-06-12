using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyBossManager enemyBossManager;
    EnemyManager enemyManager;
    EnemyEffectManager enemyEffectManager;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        enemyBossManager = GetComponent<EnemyBossManager>();
        enemyManager = GetComponent<EnemyManager>();
        enemyEffectManager = GetComponent<EnemyEffectManager>();
    }

    public void AwardSoulsOnDeath()
    {
        PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
        SoulCount soulsCout = FindObjectOfType<SoulCount>();

        if (playerStats != null)
        {
            playerStats.AddSouls(characterStatsManager.soulsAwardedOnDeath);

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

    public void PlayWeaponTrailFX()
    {
        enemyEffectManager.PlayWeaponFX(false);
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidbody.velocity = velocity * enemyManager.moveSpeed;

        if (enemyManager.isRotatingWithRootMotion)
        {
            enemyManager.transform.rotation *= animator.deltaRotation;
        }
    }
}
