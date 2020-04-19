using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverExit : GameStateRoutine
{
    public override void Run()
    {
        GameManager.instance.gameStateStarted = false;
        GameManager.instance.treeAreaSprite.sprite = GameManager.instance.treeAreaNormal;
        HideGameOverScreen();
        StopGameOverAnimation();
    }

    void HideGameOverScreen()
    {

    }

    void StopGameOverAnimation()
    {

    }
}
