using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughFogWall : Interactable
{
    [SerializeField] WorldEventManager worldEventManager;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        playerManager.PassThroughtFogWallInteraction(transform);
        worldEventManager.ActiveBossFight();
    }
}
