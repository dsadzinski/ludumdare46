using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExecute : GameStateRoutine
{

    bool finalBoss = false;
    bool spawnedBoss = false;

    float intervalDuration = 5f;
    float intervalTimer;
    
    public GameRunningState gameRunningState = GameRunningState.wave;

    public enum GameRunningState
    {
        wave,
        interval
    }

    public override void Run()
    {
        if(GameManager.instance.gameLevel == GameManager.instance.lastLevel && 
           GameManager.instance.waveNumber == GameManager.instance.wavesPerGameLevel)
        {
            finalBoss = true;
        }

        
        switch (gameRunningState)
        {
            case GameRunningState.wave:

                Debug.Log("Level " + GameManager.instance.gameLevel);
                Debug.Log("Wave " + GameManager.instance.waveNumber);

                GameManager.instance.stateText = "-  " + "Level " + GameManager.instance.gameLevel + " : " + "Wave " + GameManager.instance.waveNumber + "  -";

                if (GameManager.instance.waveTimer >= GameManager.instance.waveDuration)
                {
                    GameManager.instance.canSpawn = false;
                    OilManager.instance.canGenerateOil = false;

                    if (EnemiesManager.instance.enemiesOnScreen == 0)
                    {
                        UpgradesManager.instance.canGeneratePoints = false;
                        intervalTimer = 0f;
                        GameManager.instance.waveNumber++;
                        if (GameManager.instance.waveNumber > GameManager.instance.wavesPerGameLevel)
                        {
                            GameManager.instance.waveNumber = 1;
                            GameManager.instance.gameLevel++;
                        }
                        if(GameManager.instance.gameLevel >= 6)
                        {
                            GameManager.instance.ChangeGameState(GameManager.GameState.endGame);
                        }
                        else
                        {
                            gameRunningState = GameRunningState.interval;
                        }
                    }
                }
                else {

                    if(GameManager.instance.waveNumber == GameManager.instance.wavesPerGameLevel && !spawnedBoss)
                    {
                        OilManager.instance.oilLevelMaxedCount++;
                        if (GameManager.instance.gameLevel == GameManager.instance.lastLevel)
                        {
                            EnemiesManager.instance.SpawnFinalBoss();
                        }
                        else
                        {
                            EnemiesManager.instance.SpawnBoss(OilManager.instance.oilLevelMaxedCount);
                        }
                        spawnedBoss = true;
                    }

                    Player.instance.canCastAbilities = true;
                    GameManager.instance.canSpawn = true;
                    OilManager.instance.canGenerateOil = true;
                    GameManager.instance.waveTimer += Time.deltaTime;
                }

                if(Player.instance.currentLifes <= 0)
                {

                }

                break;

            case GameRunningState.interval:

                Debug.Log("Interval!");

                GameManager.instance.stateText = "-  INTERVAL! Next: " + "Level " + GameManager.instance.gameLevel + 
                                                " | " + "Wave " + GameManager.instance.waveNumber + "  -";

                intervalTimer += Time.deltaTime;
                Player.instance.canCastAbilities = false;

                if (intervalTimer >= intervalDuration)
                {
                    gameRunningState = GameRunningState.wave;
                    GameManager.instance.canSpawn = true;
                    OilManager.instance.canGenerateOil = true;
                    UpgradesManager.instance.canGeneratePoints = true;
                    Player.instance.canCastAbilities = true;
                    GameManager.instance.waveTimer = 0f;
                    spawnedBoss = false;
                }

                break;
            default:
                break;
        }

        if(Player.instance.currentLifes <= 0)
        {
            Debug.Log("GAME OVER!");
            GameManager.instance.ChangeGameState(GameManager.GameState.gameOver);
        }

    }


}
