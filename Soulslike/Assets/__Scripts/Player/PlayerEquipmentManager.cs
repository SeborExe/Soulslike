using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    [SerializeField] BlockingCollider blockingCollider;
    PlayerInventoryManager playerInventoryManager;
    InputHandler inputHandler;
    PlayerStatsManager playerStatsManager;

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
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        inputHandler = GetComponent<InputHandler>();
        playerStatsManager = GetComponent<PlayerStatsManager>();

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
        if (playerInventoryManager.currentHelmetEquipment != null)
        {
            helmetModelChanger.EquipHelmetModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
            playerStatsManager.physicalDamageAbsorbtionHead = playerInventoryManager.currentHelmetEquipment.physicsDefense;
        }
        else
        {
            playerInventoryManager.currentHelmetEquipment = null;
            playerStatsManager.physicalDamageAbsorbtionHead = 0f;
        }

        //Torso
        torsoModelChanger.UnEquipAllTorsoModels();
        if (playerInventoryManager.currentTorsoEquipment != null)
        {
            torsoModelChanger.EquipTorsoModelByName(playerInventoryManager.currentTorsoEquipment.torsoModelName);
            playerStatsManager.physicalDamageAbsorbtionTorso = playerInventoryManager.currentTorsoEquipment.physicsDefense;
        }
        else
        {
            playerInventoryManager.currentTorsoEquipment = null;
            playerStatsManager.physicalDamageAbsorbtionTorso = 0f;
        }

        //Pants
        pantsModelChanger.UnEquipAllPantsModels();
        if (playerInventoryManager.currentHelmetEquipment != null)
        {
            pantsModelChanger.EquipPantsModelByName(playerInventoryManager.currentPantsEquipments.pantsModelName);
            playerStatsManager.physicalDamageAbsorbtionPants = playerInventoryManager.currentPantsEquipments.physicsDefense;
        }
        else
        {
            playerInventoryManager.currentPantsEquipments = null;
            playerStatsManager.physicalDamageAbsorbtionPants = 0f;
        }

        //Boots
        bootsModelChanger.UnEquipAllBootsModels();
        if (playerInventoryManager.currentBootsEquipment != null)
        {
            bootsModelChanger.EquipBootsModelByName(playerInventoryManager.currentBootsEquipment.bootsModelName);
            playerStatsManager.physicalDamageAbsorbtionBoots = playerInventoryManager.currentGlovesEquipment.physicsDefense;
        }
        else
        {
            playerInventoryManager.currentBootsEquipment = null;
            playerStatsManager.physicalDamageAbsorbtionGloves = 0f;
        }

        //Gloves
        glovesModelChanger.UnEquipAllGlovesModels();
        if (playerInventoryManager.currentGlovesEquipment != null)
        {
            glovesModelChanger.EquipGlovesModelByName(playerInventoryManager.currentGlovesEquipment.glovesModelName);
            playerStatsManager.physicalDamageAbsorbtionGloves = playerInventoryManager.currentGlovesEquipment.physicsDefense;
        }
        else
        {
            playerInventoryManager.currentGlovesEquipment = null;
            playerStatsManager.physicalDamageAbsorbtionGloves = 0f;
        }
    }

    public void OpenBlockingCollider()
    {
        if (inputHandler.twoHandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.leftWeapon);
        }

        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
