using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExecute : GameStateRoutine
{
    bool movementTutorialComplete = false;
    bool allAbilitiesTutorialComplete = false;
    bool specialAbilitiesTutorialComplete = false;
    bool resourcesTutorialComplete = false;

    public override void Run()
    {
        GameManager.instance.isAnimatingText = true;
        GameManager.instance.canSpawn = false;
        Player.instance.canCastAbilities = true;
        OilManager.instance.canGenerateOil = false;

        UpgradesManager.instance.InitializePoints(300);

        if (!movementTutorialComplete)
        {
            GameManager.instance.stateText = "Tutorial";
            ShowMovementTutorial();
        }

        if (movementTutorialComplete && !allAbilitiesTutorialComplete)
        {
            GameManager.instance.stateText = "Test all abilities to continue";
            ShowAbilitiesTutorial();
        }

        if (allAbilitiesTutorialComplete && !specialAbilitiesTutorialComplete)
        {
            GameManager.instance.stateText = "Test a special abilitie to continue";
            ShowSpecialAbilitiesTutorial();
        }

        if(specialAbilitiesTutorialComplete && !resourcesTutorialComplete)
        {
            GameManager.instance.stateText = "Press space to start the game";
            ShowResourcesTutorial();
        }

        if (resourcesTutorialComplete)
        {
            GameManager.instance.ChangeGameState(GameManager.GameState.game);
        }

    }

    public void HideAllTutorials()
    {
        //hide all tutorials
    }

    public void ShowMovementTutorial()
    {
        GameManager.instance.MovementTutorial.SetActive(true);
        Player.instance.PlayMovementTutorialAnimation();

        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            GameManager.instance.MovementTutorial.SetActive(false);
            Player.instance.StopAnimations();
            movementTutorialComplete = true;
        }

    }

    public void ShowAbilitiesTutorial()
    {
        GameManager.instance.AbilitiesTutorial.SetActive(true);
        var shoot = Player.instance.shoot;

        allAbilitiesTutorialComplete = shoot.castVolcanoOnce && shoot.castTornadoOnce && shoot.castEarthquakeOnce && shoot.shotPestOnce && shoot.shotVirusOnce;
    }

    public void ShowSpecialAbilitiesTutorial()
    {
        Debug.Log("Show Special Abilities Tutorial");
        GameManager.instance.AbilitiesTutorial.SetActive(false);
        GameManager.instance.SpecialAbilitiesTutorial.SetActive(true);
        if (FindObjectOfType<GlobalEarthquake>() != null)
        {
            specialAbilitiesTutorialComplete = true;
            GameManager.instance.SpecialAbilitiesTutorial.SetActive(false);
        }
    }

    public void ShowResourcesTutorial()
    {
        Debug.Log("Show Resource Tutorial");
        GameManager.instance.ResourcesTutorial.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            resourcesTutorialComplete = true;
            GameManager.instance.ResourcesTutorial.SetActive(false);
        }
    }

}
