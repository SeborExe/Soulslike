using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    public Image leftWeaponIcon;
    public Image rightWeaponIcon;
    public Image currentSpellIcon;
    public Image currentConsumableItemIcon;
     
    public void UpdateWeaponSlotsUI(bool isLeft, WeaponItem weapon)
    {
        if (!isLeft)
        {
            //rightWeaponIcon.gameObject.SetActive(true);

            if (weapon.itemIcon != null)
            {
                rightWeaponIcon.sprite = weapon.itemIcon;
                rightWeaponIcon.enabled = true;
            }
            else
            {
                rightWeaponIcon.sprite = null;
                rightWeaponIcon.enabled = false;
            }

        }
        else
        {
            //leftWeaponIcon.gameObject.SetActive(true);

            if (weapon.itemIcon != null)
            {
                leftWeaponIcon.sprite = weapon.itemIcon;
                leftWeaponIcon.enabled = true;
            }
            else
            {
                leftWeaponIcon.sprite = null;
                leftWeaponIcon.enabled = false;
            }

        }
    }

    public void UpdateCurrentSpellIcon(SpellItem spellItem)
    {
        if (spellItem.itemIcon != null)
        {
            Debug.Log("Update_Spell");
            currentSpellIcon.sprite = spellItem.itemIcon;
            currentSpellIcon.enabled = true;
        }
        else
        {
            currentSpellIcon.sprite = null;
            currentSpellIcon.enabled = false;
        }
    }

    public void UpdateCurrentConsumableIcon(ConsumableItem consumable)
    {
        if (consumable.itemIcon != null)
        {
            Debug.Log("Update_Item");
            currentConsumableItemIcon.sprite = consumable.itemIcon;
            currentConsumableItemIcon.enabled = true;
        }
        else
        {
            currentConsumableItemIcon.sprite = null;
            currentConsumableItemIcon.enabled = false;
        }
    }
}
