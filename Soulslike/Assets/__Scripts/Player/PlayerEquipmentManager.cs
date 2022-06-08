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
    PantsModelChanger pantsModelChanger;
    BootsModelChanger bootsModelChanger;
    GlovesModelChanger glovesModelChanger;

    [Header("Default naked model")] //Now we have basic naked model but mayby in future we will change model and this will be helpful
    string nakedHeadModel;
    //naked torso etc. 

    private void Awake()
    {
        playerInventory = GetComponentInParent<PlayerInventory>();
        inputHandler = GetComponentInParent<InputHandler>();
        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
        torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
        pantsModelChanger = GetComponentInChildren<PantsModelChanger>();
        bootsModelChanger = GetComponentInChildren<BootsModelChanger>();
        glovesModelChanger = GetComponentInChildren<GlovesModelChanger>();
    }

    private void Start()
    {
        EquipAllEquipmentModelsOnStart();
    }

    private void EquipAllEquipmentModelsOnStart()
    {
        //Helmet
        helmetModelChanger.UnEquipAllHelmetModels();
        if (playerInventory.currentHelmetEquipment != null)
            helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
        else
            playerInventory.currentHelmetEquipment = null;

        //Torso
        torsoModelChanger.UnEquipAllTorsoModels();
        if (playerInventory.currentTorsoEquipment != null)
            torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
        else
            playerInventory.currentTorsoEquipment = null;

        //Pants
        pantsModelChanger.UnEquipAllPantsModels();
        if (playerInventory.currentHelmetEquipment != null)
            pantsModelChanger.EquipPantsModelByName(playerInventory.currentPantsEquipments.pantsModelName);
        else
            playerInventory.currentPantsEquipments = null;

        //Boots
        bootsModelChanger.UnEquipAllBootsModels();
        if (playerInventory.currentBootsEquipment != null)
            bootsModelChanger.EquipBootsModelByName(playerInventory.currentBootsEquipment.bootsModelName);
        else
            playerInventory.currentBootsEquipment = null;

        //Gloves
        glovesModelChanger.UnEquipAllGlovesModels();
        if (playerInventory.currentGlovesEquipment != null)
            glovesModelChanger.EquipGlovesModelByName(playerInventory.currentGlovesEquipment.glovesModelName);
        else
            playerInventory.currentGlovesEquipment = null;
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
