using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterStatsManager characterStatsManager;

    [Header("Damage FX")]
    public GameObject bloodSplatterFX;

    [Header("Weapon FX")]
    public WeaponFX rightWeaponWF;
    public WeaponFX leftWeaponWF;

    [Header("Poison")]
    public GameObject defaultPoisonParticleFX;
    public Transform buildUpTransform;
    public GameObject currentPoisonParticleFX;
    public bool isPoisoned;
    public float poisonBuildUp = 0f;
    public float poisonAmount = 100f;
    public float defaultPoisonAmount = 100f;
    public float poisonTimer = 2f;   //Time between poison tick 
    public int poisonDamage = 1;
    float timer = 0f;

    protected virtual void Awake()
    {
        characterStatsManager = GetComponent<CharacterStatsManager>();
    }

    public virtual void PlayWeaponFX(bool isLeft)
    {
        if (!isLeft)
        {
            if (rightWeaponWF != null)
            {
                rightWeaponWF.PlayWeaponFX();
            }
        }
        else
        {
            if (leftWeaponWF != null)
            {
                leftWeaponWF.PlayWeaponFX();
            }
        }
    }

    public virtual void PlayBloodSplatterFX(Vector3 bloodSplaterLocation)
    {
        GameObject blood = Instantiate(bloodSplatterFX, bloodSplaterLocation, Quaternion.identity);
    }

    public virtual void HandleAllBuildUpEffects()
    {
        if (characterStatsManager.isDead) return;

        HandlePoisonBuildUp();
        HadleIsPoisonedEffect();
    }

    protected virtual void HandlePoisonBuildUp()
    {
        if (isPoisoned) return;

        if (poisonBuildUp > 0 && poisonBuildUp < 100)
        {
            poisonBuildUp = poisonBuildUp - 1 * Time.deltaTime;
        }

        else if (poisonBuildUp >= 100)
        {
            isPoisoned = true;
            poisonBuildUp = 0;

            if (buildUpTransform != null)
            {
                currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, buildUpTransform.transform);
            }
            else
            {
                currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, characterStatsManager.transform);
            }
        }
    }

    protected virtual void HadleIsPoisonedEffect()
    {
        if (isPoisoned)
        {
            if (poisonAmount > 0)
            {
                timer += Time.deltaTime;

                if (timer >= poisonTimer)
                {
                    characterStatsManager.TakePoisonDamage(poisonDamage);
                    timer = 0;
                }

                poisonAmount = poisonAmount - 1 * Time.deltaTime;
            }
            else
            {
                isPoisoned = false;
                poisonAmount = defaultPoisonAmount;
                Destroy(currentPoisonParticleFX);
            }
        }
    }
}
