using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryManager : MonoBehaviour
{
    protected CharacterWeaponSlotManager characterWeaponSlotManager;

    [Header("Quick Slots Items")]
    public ConsumableItem currentConsumableItem;
    public SpellItem currentSpell;
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;

    [Header("Current Equipment")]
    public HelmetEquipment currentHelmetEquipment;
    public TorsoEquipment currentTorsoEquipment;
    public PantsEquipment currentPantsEquipments;
    public BootsEquipment currentBootsEquipment;
    public GlovesEquipment currentGlovesEquipment;

    [Header("EQ weapons")]
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;

    private void Awake()
    {
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
    }

    private void Start()
    {
        characterWeaponSlotManager.LoadBothWeaponsOnSlot();
    }
}
