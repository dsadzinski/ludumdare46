using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UpgradesManager : MonoBehaviour
{
    GameManager gameManager;
    OilManager oilManager;

    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] TextMeshProUGUI treeCostText;

    [SerializeField] Button TRexUnlockButton;
    [SerializeField] Button ETUnlockButton;

    [SerializeField] Button TRexSpawnButton;
    [SerializeField] Button ETSpawnButton;

    [SerializeField] GameObject tree;
    List<GameObject> currentTreesGO = new List<GameObject>();

    [SerializeField] Vector2 bottomLeftTreePosition;
    [SerializeField] Vector2 topRightTreePosition;

    int initialPoints = 250;
    public int points { get; private set; }
    public int pointGenerationRate { get; private set; }
    int basePointGenerationRate = 5;
    public float pointTickCooldown { get; private set; } = 0.5f;
    float pointTickTimer;

    int tRexUnlockCost = 800;
    int ETUnlockCost = 2300;

    public bool canGeneratePoints;

    public int globalEarthquakeSpawnCost = 250;
    public int tRexSpawnCost = 650;
    public int ETSpawnCost = 1600;

    int treeAmount;
    int treeCost = 10;
    float treeHealRate = 0.5f;
    int treeGenerationRate = 3;
    float treeOilGenerationRate = 0.15f;

    #region SINGLETON
    public static UpgradesManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Points Manager!!");
            return;
        }
        instance = this;

    }
    #endregion

    public void Start()
    {
        InitializePoints(100);
        treeAmount = 0;
        treeCost = 10;

        RemoveAllTrees();

        canGeneratePoints = false;

        gameManager = GameManager.instance;
        oilManager = OilManager.instance;
        pointGenerationRate = basePointGenerationRate;

        TRexUnlockButton.gameObject.SetActive(true);
        ETUnlockButton.gameObject.SetActive(true);

        TRexSpawnButton.gameObject.SetActive(false);
        ETSpawnButton.gameObject.SetActive(false);

        treeAmount = 0;
    }

    void Update()
    {
        pointTickTimer += Time.deltaTime;

        treeCost = 10 + treeAmount * treeAmount * 2;

        if (canGeneratePoints)
        {
            GeneratePoints();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlantTree();
        }

        pointsText.text = "Green Points: " + points;
        treeCostText.text = "Cost: " + treeCost;
    }

    public void RemoveAllTrees()
    {
        if (currentTreesGO != null)
        {
            foreach (GameObject t in currentTreesGO)
            {
                Destroy(t);
               
            }
        }
    }

    public void GeneratePoints()
    {
        if (pointTickTimer >= pointTickCooldown)
        {
            pointTickTimer = 0f;
            points += pointGenerationRate;
            Player.instance.Heal(currentTreesGO.Count * treeHealRate);
        }
    }

    public void InitializePoints(int p)
    {
        points = p;
    }

    public void AddPoints(int amount)
    {
        points += amount;
    }

    public void RemovePoints(int amount)
    {
        AddPoints(-amount);
    }

    public void IncreaseGenerationRate(int rate)
    {
        pointGenerationRate += rate;
    }

    public void DecreaseGenerationRate(int rate)
    {
        IncreaseGenerationRate(-rate);
    }

    public void PlantTree(int i = 1)
    {
        if (points >= treeCost)
        {
            treeAmount += i;
            RemovePoints(treeCost);
            Debug.Log("Plant Tree");
            SpawnTree();
            IncreaseGenerationRate(i * treeGenerationRate);
            oilManager.IncreaseOilGenerationRate(i * treeOilGenerationRate);
        }
    }

    public void SpawnTree()
    {
        float xSpawn = Random.Range(bottomLeftTreePosition.x, topRightTreePosition.x);
        float ySpawn = Random.Range(bottomLeftTreePosition.y, topRightTreePosition.y);

        Vector2 spawnPosition = new Vector2(xSpawn, ySpawn);
        GameObject treeInst = Instantiate(tree, spawnPosition, Quaternion.identity, this.transform);
        currentTreesGO.Add(treeInst);
    }

    public void ChopTree(int amount)
    {
        if (treeAmount > 0)
        {
            treeAmount -= amount;
            DecreaseGenerationRate(amount * treeGenerationRate);
            oilManager.DecreaseOilGenerationRate(amount * treeOilGenerationRate);
            int treeToDestroy = Random.Range(0, currentTreesGO.Count);
            //currentTreesGO.RemoveAt(treeToDestroy);
            Destroy(currentTreesGO[treeToDestroy]);           
        }
    }

    public void UnlockTRex()
    {
        if(points >= tRexUnlockCost)
        {
            RemovePoints(tRexUnlockCost);
            TRexSpawnButton.gameObject.SetActive(true);
            TRexUnlockButton.gameObject.SetActive(false);
        }
    }

    public void UnlockET()
    {
        if (points >= tRexUnlockCost)
        {
            RemovePoints(ETUnlockCost);
            ETSpawnButton.gameObject.SetActive(true);
            ETUnlockButton.gameObject.SetActive(false);
        }
    }
}
