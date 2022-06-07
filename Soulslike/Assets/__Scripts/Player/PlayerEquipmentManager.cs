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
    TorsoModelChanger torsoModelChanger;

    [Header("Default naked model")] //Now we have basic naked model but mayby in future we will change model and this will be helpful
    string nakedHeadModel;
    //naked torso etc. 

    private void Awake()
    {
        playerInventory = GetComponentInParent<PlayerInventory>();
        inputHandler = GetComponentInParent<InputHandler>();
        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
        torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
    }

    private void Start()
    {
        EquipAllEquipmentModelsOnStart();
    }

    private void EquipAllEquipmentModelsOnStart()
    {
        helmetModelChanger.UnEquipAllHelmetModels();
        if (playerInventory.currentHelmetEquipment != null)
            helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
        else
            playerInventory.currentHelmetEquipment = null;

        torsoModelChanger.UnEquipAllTorsoModels();
        if (playerInventory.currentTorsoEquipment != null)
            torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
        else
            playerInventory.currentTorsoEquipment = null;
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
