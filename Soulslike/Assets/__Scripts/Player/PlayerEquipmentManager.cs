using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    [SerializeField] BlockingCollider blockingCollider;
    PlayerInventory playerInventory;
    InputHandler inputHandler;
    PlayerStats playerStats;

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
        playerStats = GetComponentInParent<PlayerStats>();

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
        {
            helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
            playerStats.physicalDamageAbsorbtionHead = playerInventory.currentHelmetEquipment.physicsDefense;
        }
        else
        {
            playerInventory.currentHelmetEquipment = null;
            playerStats.physicalDamageAbsorbtionHead = 0f;
        }

        //Torso
        torsoModelChanger.UnEquipAllTorsoModels();
        if (playerInventory.currentTorsoEquipment != null)
        {
            torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
            playerStats.physicalDamageAbsorbtionTorso = playerInventory.currentTorsoEquipment.physicsDefense;
        }
        else
        {
            playerInventory.currentTorsoEquipment = null;
            playerStats.physicalDamageAbsorbtionTorso = 0f;
        }

        //Pants
        pantsModelChanger.UnEquipAllPantsModels();
        if (playerInventory.currentHelmetEquipment != null)
        {
            pantsModelChanger.EquipPantsModelByName(playerInventory.currentPantsEquipments.pantsModelName);
            playerStats.physicalDamageAbsorbtionPants = playerInventory.currentPantsEquipments.physicsDefense;
        }
        else
        {
            playerInventory.currentPantsEquipments = null;
            playerStats.physicalDamageAbsorbtionPants = 0f;
        }

        //Boots
        bootsModelChanger.UnEquipAllBootsModels();
        if (playerInventory.currentBootsEquipment != null)
        {
            bootsModelChanger.EquipBootsModelByName(playerInventory.currentBootsEquipment.bootsModelName);
            playerStats.physicalDamageAbsorbtionBoots = playerInventory.currentGlovesEquipment.physicsDefense;
        }
        else
        {
            playerInventory.currentBootsEquipment = null;
            playerStats.physicalDamageAbsorbtionGloves = 0f;
        }

        //Gloves
        glovesModelChanger.UnEquipAllGlovesModels();
        if (playerInventory.currentGlovesEquipment != null)
        {
            glovesModelChanger.EquipGlovesModelByName(playerInventory.currentGlovesEquipment.glovesModelName);
            playerStats.physicalDamageAbsorbtionGloves = playerInventory.currentGlovesEquipment.physicsDefense;
        }
        else
        {
            playerInventory.currentGlovesEquipment = null;
            playerStats.physicalDamageAbsorbtionGloves = 0f;
        }
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
