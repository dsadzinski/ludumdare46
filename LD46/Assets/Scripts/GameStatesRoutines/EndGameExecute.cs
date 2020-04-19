using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameExecute : GameStateRoutine
{
    public override void Run()
    {
        GameManager.instance.isAnimatingText = true;
        GameManager.instance.stateText = "Press ESC to exit game.";
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
