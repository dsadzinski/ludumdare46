using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverExecute : GameStateRoutine
{
    public override void Run()
    {
        GameManager.instance.stateText = "GAME OVER!! - Press Space to restart";
        GameManager.instance.isAnimatingText = true;
        UpgradesManager.instance.RemoveAllTrees();
        GameManager.instance.treeAreaSprite.sprite = GameManager.instance.treeAreaGameOver;
        
        RunGameOverAnimation();

        if (Input.GetKey(KeyCode.Space))
        {
            GameManager.instance.ChangeGameState(GameManager.GameState.game);
            GameManager.instance.RestartGame();
        }
    }

    void RunGameOverAnimation()
    {
        GameManager.instance.earthAnim.Play("Earth-GameOver");
    }

}
