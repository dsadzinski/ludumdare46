using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilManager : MonoBehaviour
{
    
    public float oilLevel { get; private set; }

    public bool canGenerateOil;

    public int oilLevelMaxedCount;

    float oilTickCooldown = 1f;
    float oilTickTimer;

    public float baseOilGenerationRate { get; private set; } = 0.8f;
    public float oilGenerationRate { get; private set; }

    public float maxOil { get; private set; } = 100f;

    public delegate void OnOilLevelChanged(float oilLevel, int oilLevelMaxxedCount);
    public OnOilLevelChanged OnOilLevelChangedCallback;

    public delegate void OnOilLevelMaxed(int oilLevelMaxxedCount);
    public OnOilLevelMaxed OnOilLevelMaxedCallback;

    #region SINGLETON
    public static OilManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one Game Manager!");
        }

        instance = this;

    }
    #endregion

    public void Start()
    {
        oilGenerationRate = baseOilGenerationRate;
        oilTickTimer = 0f;
        oilLevel = 0f;
        oilLevelMaxedCount = 0;
    }
  
    void Update()
    {
        if (canGenerateOil)
        {
            oilTickTimer += Time.deltaTime;
            GenerateOil();
        }
    }

    public void SetGenerationRate(float g)
    {
        oilGenerationRate = g;
    }

    public void GenerateOil()
    {
        if (oilTickTimer >= oilTickCooldown)
        {
            IncreaseOilLevel(oilGenerationRate);
            ResetOilTickTimer();
        }
        
    }

    public void ResetOilTickTimer()
    {
        oilTickTimer = 0f;
    }

    public void ResetOilManager()
    {
        oilGenerationRate = baseOilGenerationRate;
        oilTickTimer = 0f;
        oilLevel = 0f;
        oilLevelMaxedCount = 0;
    }

    public void ResetOilLevel()
    {
        oilLevel = 0; ;
    }

    public void IncreaseOilLevel(float amount)
    {
        oilLevel += amount;
        if (oilLevel >= maxOil)
        {
            ReachedMaxOil();
        }
        OnOilLevelChangedCallback.Invoke(oilLevel, oilLevelMaxedCount);
    }

    public void DecreaseOilLevel(float amount)
    {
        IncreaseOilLevel(-amount);
        if(oilLevel < 0)
        {
            oilLevel = 0;
        }
    }

    public void IncreaseOilGenerationRate(float amount)
    {
        oilGenerationRate += amount;
    }

    public void DecreaseOilGenerationRate(float amount)
    {
        oilGenerationRate -= amount;
    }

    public void ReachedMaxOil()
    {
        DecreaseOilLevel(maxOil);
        oilLevelMaxedCount++;      
        //Debug.Log("SpawnBoss");
        OnOilLevelMaxedCallback(oilLevelMaxedCount);
    }
}
