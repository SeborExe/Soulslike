using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public Transform parentOverride;
    public Transform parentShieldOverride;
    public WeaponItem currentWeapon;
    public bool isLeftHandSlot;
    public bool isRightHandSlot;
    public bool isBackSlot;

    public GameObject currentWeaponModel;

    public void UnloadWeapon()
    {
        if (currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);
        }
    }

    public void UnloadWeaponAndDestroy()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        UnloadWeaponAndDestroy();

        if (weaponItem == null)
        {
            UnloadWeapon();
            return;
        }

        GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
        if (model != null)
        {
            if (parentShieldOverride != null && weaponItem.weaponType == WeaponType.Shield)
            {
                model.transform.parent = parentShieldOverride;
            }
            else if (parentOverride != null && weaponItem.weaponType != WeaponType.Shield)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            //model.transform.localScale = Vector3.one * 0.05f; //Because our prefab is big
            model.transform.localScale = Vector3.one;
        }

        currentWeaponModel = model;
    }
}

