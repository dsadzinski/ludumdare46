using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : GameStateRoutine
{
 
    public override void Run()
    {
        GameManager.instance.earthAnim.Play("Earth-Normal");

        Player.instance.RemoveTRex();
        GameManager.instance.gameLevel = 1;
        GameManager.instance.waveNumber = 1;
        UpgradesManager.instance.InitializePoints(100);
        GameManager.instance.canSpawn = true;
        OilManager.instance.canGenerateOil = true;
        GameManager.instance.isAnimatingText = false;

        Debug.Log("Started Game");
    }

}
