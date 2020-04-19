using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStart : GameStateRoutine
{
    public override void Run()
    {
        GameManager.instance.canSpawn = false;
        Player.instance.canCastAbilities = true;
        OilManager.instance.canGenerateOil = false;
        UpgradesManager.instance.InitializePoints(300);
        Debug.Log("Started Tutorial");
    }

}
