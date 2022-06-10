using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    //Fog wall
    [SerializeField] UIBossHealthBar bossHealthBar;
    [SerializeField] EnemyBossManager boss;
    public List<FogWall> fogWalls;

    public bool bossFightIsActive; //boss fight is in progress
    public bool bossHasBeenAwakened; //woke the boss, or cutscene
    public bool bossHasBeenDefeded;

    public void ActiveBossFight()
    {
        bossFightIsActive = true;
        bossHasBeenAwakened = true;
        bossHealthBar.SetUIHealthBarToActive();

        foreach (FogWall fogwall in fogWalls)
        {
            fogwall.ActivateFogWall();
        }
    }

    public void BossHasBeenDefeded()
    {
        bossFightIsActive = false;
        bossHasBeenDefeded = true;

        foreach (FogWall fogwall in fogWalls)
        {
            fogwall.DesactiveFogWall();
        }
    }

}
