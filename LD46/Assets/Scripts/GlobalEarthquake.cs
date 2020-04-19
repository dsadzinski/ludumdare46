using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEarthquake : MonoBehaviour
{
    [SerializeField] GameObject EarthquakePrefab;

    float duration = 3.5f;
    float timer;
    int maxEarthquakesToSpawn = 5;

    int currentEarthquakeAmount = 0;

    float spawnEarthquakeInterval = 0.6f;

    float damage;
    float oilLeak;

    bool generating;

    Vector2 topLeftLimit = new Vector2(1.92f, 1.3f);
    Vector2 bottomRightLimit = new Vector2(-1.7f, 0.38f);

    void Start()
    {
        currentEarthquakeAmount = 0;
        timer = 0;
        generating = false;

        if (GameManager.instance.gameState == GameManager.GameState.game)
        {
            damage = (maxEarthquakesToSpawn * duration) / 2;
            oilLeak = (maxEarthquakesToSpawn * duration) / 1.6f;
        }

        Player.instance.ApplyDamage(damage);
        OilManager.instance.DecreaseOilLevel(oilLeak);
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if (!generating && currentEarthquakeAmount < maxEarthquakesToSpawn)
        {
            Debug.Log("Spawn Earthquake");
            //GenerateEarthquake();
            StartCoroutine(GenerateEarthquake());
        }

        currentEarthquakeAmount = transform.GetComponentsInChildren<Earthquake>().Length;

        if (timer >= duration)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator GenerateEarthquake()
    {
        var amountToSpawn = maxEarthquakesToSpawn - currentEarthquakeAmount;

        for (int i = 0; i < amountToSpawn; i++)
        {
            generating = true;
            float xSpawn = Random.Range(bottomRightLimit.x, topLeftLimit.x);
            float ySpawn = Random.Range(bottomRightLimit.y, topLeftLimit.y);

            GameObject earthquakeInst = Instantiate(EarthquakePrefab, new Vector3(xSpawn, ySpawn, 0), Quaternion.identity, transform);
            earthquakeInst.GetComponent<Earthquake>().StartTimer(Player.instance.shoot.earthquakeTimer);

            currentEarthquakeAmount = transform.GetComponentsInChildren<Earthquake>().Length;

            yield return new WaitForSeconds(spawnEarthquakeInterval);
            generating = false;
        }

        
    }

}
