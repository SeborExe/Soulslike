using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    [Header("Damage FX")]
    public GameObject bloodSplatterFX;

    [Header("Weapon FX")]
    public WeaponFX rightWeaponWF;
    public WeaponFX leftWeaponWF;

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
}
