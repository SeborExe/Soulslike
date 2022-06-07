using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    [SerializeField] BlockingCollider blockingCollider;
    PlayerInventory playerInventory;
    InputHandler inputHandler;

    [Header("Equipment model changers")]
    HelmetModelChanger helmetModelChanger;

    private void Awake()
    {
        playerInventory = GetComponentInParent<PlayerInventory>();
        inputHandler = GetComponentInParent<InputHandler>();
        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
    }

    private void Start()
    {
        helmetModelChanger.UnEquipAllHelmetModels();
        helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
    }

    public void OpenBlockingCollider()
    {
        if (inputHandler.twoHandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);
        }

        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
