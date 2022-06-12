using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
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
}
