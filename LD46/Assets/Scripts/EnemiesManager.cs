using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{

    [SerializeField] GameObject simpleEnemy;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject finalBoss;

    [SerializeField] Transform maxYPositionToSpawn;
    [SerializeField] Transform minYPositionToSpawn;
    [SerializeField] Transform xPositionToSpawn;

    float maxY;
    float minY;
    float x;

    float baseSpawnCooldown = 5f;
    float spawnCooldown;
    float spawnTimer = 0f;

    public bool isFinalBoss;

    public List<GameObject> currentEnemiesOnScreen = new List<GameObject>();
    public int enemiesOnScreen;

    public static EnemiesManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one enemy manager.");
            return;
        }
        instance = this;

    }

    public void Start()
    {
        x = xPositionToSpawn.position.x;
        minY = minYPositionToSpawn.position.y;
        maxY = maxYPositionToSpawn.position.y;

        isFinalBoss = false;

        spawnCooldown = baseSpawnCooldown;

        if(currentEnemiesOnScreen != null)
        {
            foreach(GameObject e in currentEnemiesOnScreen)
            {
                Destroy(e);
            }
        }

        OilManager.instance.OnOilLevelChangedCallback += CheckOilLevel;
        OilManager.instance.OnOilLevelMaxedCallback += SpawnBoss;

    }

    // Update is called once per frame
    void Update()
    {
        enemiesOnScreen = GetComponentsInChildren<Enemy>().Length;

        if (GameManager.instance.canSpawn)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnCooldown)
            {
                spawnTimer = 0f;

                if (isFinalBoss)
                {
                    SpawnFinalBoss();
                }
                StartCoroutine(SpawnEnemy(GameManager.instance.gameLevel));
            }
        }

        if (Input.GetKeyDown(KeyCode.End))
        {
            SpawnFinalBoss();
        }

    }

    public void SpawnFinalBoss()
    {
        float ySpawn = 0.8f;
        GameObject bossInst = Instantiate(finalBoss, new Vector3(x, ySpawn, 0), Quaternion.identity, this.transform);
        currentEnemiesOnScreen.Add(bossInst);
        bossInst.transform.GetComponent<Boss>().Initialize(OilManager.instance.oilLevelMaxedCount, true);
    }

    void CheckOilLevel(float oilLevel, int oilLevelMaxxedCount)
    {
        spawnCooldown = baseSpawnCooldown - (oilLevel / 80f) - (((float)oilLevelMaxxedCount)/14f);
    }

    public void SpawnBoss(int maxedCount)
    {
        bool finalBoss = maxedCount >= 10;
        
        float ySpawn = (maxY - minY) / 2;
        GameObject bossInst = Instantiate(boss, new Vector3(x, ySpawn, 0), Quaternion.identity,this.transform);
        currentEnemiesOnScreen.Add(bossInst);
        bossInst.transform.GetComponent<Boss>().Initialize(maxedCount, finalBoss);
    }

    public void SpawnBoss(int maxedCount, bool final)
    {
        bool finalBoss = final;

        float ySpawn = (maxY - minY) / 2;
        GameObject bossInst = Instantiate(boss, new Vector3(x, ySpawn, 0), Quaternion.identity, this.transform);
        currentEnemiesOnScreen.Add(bossInst);
        bossInst.transform.GetComponent<Boss>().Initialize(maxedCount, finalBoss);
    }

    IEnumerator SpawnEnemy(int level)
    {
        int amountToSpawn = 3 + (level * (10 / 4)) + Mathf.CeilToInt(OilManager.instance.oilLevel / 20);

        //Debug.Log("Amount to Spawn: " + amountToSpawn);

        for (int i = 0; i < amountToSpawn; i++)
        {
            float ySpawn = Random.Range(minY, maxY);

            GameObject enemyInst = Instantiate(simpleEnemy, new Vector3(x, ySpawn, 0), Quaternion.identity, this.transform);
            currentEnemiesOnScreen.Add(enemyInst);
            enemyInst.transform.GetComponent<SimpleEnemy>().Initialize(level, OilManager.instance.oilLevel);
            if (i<(amountToSpawn - 1))
            {
                //Debug.Log("Waitforspawninterval");
                yield return new WaitForSeconds(0.2f);
            }
        }
        

    }



}
