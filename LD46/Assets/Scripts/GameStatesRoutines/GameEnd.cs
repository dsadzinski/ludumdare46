using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : GameStateRoutine
{
    public override void Run()
    {
        GameManager.instance.canSpawn = false;
        Player.instance.canCastAbilities = false;
    }

}
