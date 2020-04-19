using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    public int gameLevel;
    public int lastLevel = 3;
    public bool canSpawn = false;

    public int wavesPerGameLevel = 5;
    public float waveDuration = 18f;
    public float waveTimer;

    public int waveNumber;

    public GameState gameState = GameState.tutorial;

    [SerializeField] public List<GameObject> objectsToDisableOnEndGame = new List<GameObject>();
    [SerializeField] public GameObject EndScreenText;

    [SerializeField] public SpriteRenderer treeAreaSprite;
    [SerializeField] public Sprite treeAreaNormal;
    [SerializeField] public Sprite treeAreaGameOver;

    [SerializeField] public GameObject MovementTutorial;
    [SerializeField] public GameObject AbilitiesTutorial;
    [SerializeField] public GameObject SpecialAbilitiesTutorial;
    [SerializeField] public GameObject ResourcesTutorial;

    [SerializeField] public Animator earthAnim;

    public string stateText;
    [SerializeField] public TextMeshPro gameStateText;
    public bool isAnimatingText;

    float textAnimationTimer;
    float textAnimationDuration = 0.5f;

    public bool gameStateStarted = false;

    TutorialStart tutorialStart = new TutorialStart();
    TutorialExecute tutorialExecute = new TutorialExecute();
    TutorialExit tutorialExit = new TutorialExit();
    GameStart gameStart = new GameStart();
    GameExecute gameExecute = new GameExecute();
    GameEnd gameExit = new GameEnd();
    GameOverStart gameOverStart = new GameOverStart();
    GameOverExecute gameOverExecute = new GameOverExecute();
    GameOverExit gameOverExit = new GameOverExit();
    EndGameStart endGameStart = new EndGameStart();
    EndGameExecute endGameExecute = new EndGameExecute();
    EndGameExit endGameExit = new EndGameExit();

    public enum GameState
    {
        tutorial,
        game,
        gameOver,
        endGame
    }

    public static GameManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one Game Manager!");
            return;
        }

        instance = this;
        gameState = GameState.tutorial;
        gameStateStarted = true;
    }

    void Start()
    {
        gameLevel = 1;
        waveNumber = 1;
        waveTimer = 0f;
        canSpawn = false;
        //ChangeGameState(GameState.tutorial);
    }

    
    void Update()
    {
        gameStateText.text = stateText;
        AnimateGameStateText();
        ExecuteGameState(gameState);
    }

    public void AnimateGameStateText()
    {
        if (isAnimatingText)
        {
            textAnimationTimer += Time.deltaTime;
            if (textAnimationTimer >= textAnimationDuration) {
                gameStateText.gameObject.SetActive(!gameStateText.gameObject.activeSelf);
                textAnimationTimer = 0f;
            }
        }
        else
        {
            gameStateText.gameObject.SetActive(true);
        }
    }

    public void StartGameState(GameState currentGameState)
    {
        switch (gameState)
        {
            case GameState.tutorial:
                tutorialStart.Run();
                break;
            case GameState.game:
                gameStart.Run();
                break;
            case GameState.gameOver:
                gameOverStart.Run();
                break;
            case GameState.endGame:
                endGameStart.Run();
                break;
            default:
                break;
        }

        gameStateStarted = true;

    }

    public void ExecuteGameState(GameState currentGameState)
    {
        if (gameStateStarted)
        {
            switch (gameState)
            {
                case GameState.tutorial:
                    tutorialExecute.Run();
                    break;
                case GameState.game:
                    gameExecute.Run();
                    break;
                case GameState.gameOver:
                    gameOverExecute.Run();
                    break;
                case GameState.endGame:
                    endGameExecute.Run();
                    break;
                default:
                    break;
            }
        }
    }

    public void ExitGameState(GameState currentGameState)
    {
        switch (gameState)
        {
            case GameState.tutorial:
                tutorialExit.Run();
                break;
            case GameState.game:
                gameExit.Run();
                break;
            case GameState.gameOver:
                gameOverExit.Run();
                break;
            case GameState.endGame:
                endGameExit.Run();
                break;
            default:
                break;
        }

        gameStateStarted = false;
    }

    public void ChangeGameState(GameState newState)
    {
        ExitGameState(gameState);
        gameState = newState;
        StartGameState(gameState);
    }

    public void RestartGame()
    {
        EnemiesManager.instance.Start();
        OilManager.instance.Start();
        Player.instance.Start();
        UpgradesManager.instance.Start();
        Start();
    }

}
