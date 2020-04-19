using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameStart : GameStateRoutine
{
    float endGameStartTimer = 0f;
    
    public override void Run()
    {
        GameManager.instance.gameStateText.gameObject.SetActive(false);
        Debug.Log("Started End Game");
        foreach(GameObject o in GameManager.instance.objectsToDisableOnEndGame)
        {
            o.SetActive(false);
        }
        GameManager.instance.EndScreenText.SetActive(true);
        while (endGameStartTimer <= 6f)
        {
            endGameStartTimer += Time.deltaTime;
        }
        GameManager.instance.gameStateText.gameObject.SetActive(true);
    }


}
