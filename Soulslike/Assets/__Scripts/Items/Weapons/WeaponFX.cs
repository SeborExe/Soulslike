using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [Header("Weapon FX")]
    public ParticleSystem normalWeaponTrail;
    //fire weapon trail etc.

    public void PlayWeaponFX()
    {
        normalWeaponTrail.Stop();

        if (normalWeaponTrail.isStopped)
        {
            normalWeaponTrail.Play();
        }
    }
}
