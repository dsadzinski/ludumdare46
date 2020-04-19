using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float maxHealth { get; private set; } = 100f;
    public float currentHealth;
    public int maxLifes = 3;
    public int currentLifes;

    public bool canCastAbilities = false;

    [SerializeField] Animator anim;
    [SerializeField] string normal;
    [SerializeField] string movementTutorial;

    [SerializeField] public Shoot shoot;
    [SerializeField] GlobalEarthquake globalEarthquake;
    [SerializeField] Trex TRex;

    public float globalEarthquakeCooldown = 9f;
    public float globalEarthquakeCooldownTimer;
    public float TrexCooldown = 10f;
    public float TrexCooldownTimer;
    public float ETCooldown = 7f;
    public float ETCooldownTimer;

    public bool canCastGlobalEarthquake;
    public bool canCastTrex;
    public bool canCastET;
    public bool TrexAlive;

    public enum SpecialAbility
    {
        globalEarthquakes,
        TRex,
        ET
    }

    #region SINGLETON
    public static Player instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one Player!!");
        }        
        instance = this;
    }
    #endregion
    public void Start()
    {
        globalEarthquakeCooldownTimer = globalEarthquakeCooldown;
        TrexCooldownTimer = TrexCooldown;
        ETCooldownTimer = ETCooldown;
        currentHealth = maxHealth;
        currentLifes = maxLifes;
    }

    
    void Update()
    {
        if(currentHealth <= 0)
        {
            LoseLife();
        }
        CountSpecialAbilitiesCooldown();
    }

    void CountSpecialAbilitiesCooldown()
    {
        if (!canCastGlobalEarthquake)
        {
            globalEarthquakeCooldownTimer += Time.deltaTime;
            if (globalEarthquakeCooldownTimer >= globalEarthquakeCooldown)
            {
                canCastGlobalEarthquake = true;
                globalEarthquakeCooldownTimer = globalEarthquakeCooldown;
            }
        }
        if (!canCastTrex && TrexAlive == false)
        {
            TrexCooldownTimer += Time.deltaTime;
            if (TrexCooldownTimer >= TrexCooldown)
            {
                canCastTrex = true;
                TrexCooldownTimer = TrexCooldown;
            }
        }
        if (!canCastET)
        {
            ETCooldownTimer += Time.deltaTime;
            if (ETCooldownTimer >= ETCooldown)
            {
                canCastET = true;
                ETCooldownTimer = ETCooldown;
            }
        }

    }

    public void CastGlobalEarthquake()
    {
        if (canCastAbilities && canCastGlobalEarthquake)
        {
            if (UpgradesManager.instance.points >= UpgradesManager.instance.globalEarthquakeSpawnCost)
            {
                UpgradesManager.instance.RemovePoints(UpgradesManager.instance.globalEarthquakeSpawnCost);
                globalEarthquakeCooldownTimer = 0;
                canCastGlobalEarthquake = false;
                Instantiate(globalEarthquake);
            }
        }
    }

    public void CastTRex()
    {
        if (canCastAbilities && canCastTrex)
        {
            if (UpgradesManager.instance.points >= UpgradesManager.instance.tRexSpawnCost)
            {
                UpgradesManager.instance.RemovePoints(UpgradesManager.instance.tRexSpawnCost);
                TrexCooldownTimer = 0;
                canCastTrex = false;
                Instantiate(TRex);
            }
        }
    }

    public void ApplyDamage(float d)
    {
        currentHealth -= d;
    }

    public void Heal(float d)
    {
        currentHealth += d;
    }

    public void LoseLife()
    {
        currentLifes -= 1;
        if(currentLifes <= 0)
        {
            Die();
            return;
        }
        currentHealth = maxHealth;
    }

    public void StopAnimations()
    {
        anim.Play(normal);
    }

    public void PlayMovementTutorialAnimation()
    {
        anim.Play(movementTutorial);
    }

    public void RemoveTRex()
    {
        if(FindObjectOfType<Trex>() != null)
        {
            Destroy(FindObjectOfType<Trex>().gameObject);
        }
    }

    public void Die()
    {

    }
}
