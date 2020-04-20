using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    Vector2 aimPosition;
    Vector2 aimDirection;
    Movement movement;

    public float earthquakeTimer { get; private set; } = 2.5f;
    float volcanoTimer = 1.5f;
    float tornadoSpeed = 5f;

    float virusDamage = 8f;
    float pestDamage = 12f;
    float tornadoDamage = 3f;
    float volcanoDamage = 10f;

    float tornadoCarryVelocity = 2f;

    float virusContaminationDamage = 2f;
    float virusContaminationRadius = 0.25f;
    float pestSlow = 0.85f;

    float virusShotCooldown = 0.2f;
    float virusShotTimer;
    float pestShotCooldown = 0.4f;
    float pestShotTimer;
    bool canShootVirus = true;
    bool canShootPest = true;

    public float volcanoCooldown = 2f;
    public float volcanoCooldownTimer;
    public float earthquakeCooldown = 3f;
    public float earthquakeCooldownTimer;
    public float tornadoCooldown = 4.5f;
    public float tornadoCooldownTimer;
    public bool canCastVolcano = true;
    public bool canCastEarthquake = true;
    public bool canCastTornado = true;

    float maxYAimPosition = 1.5f;
    float minYAimPosition = 0.15f;
    bool selectedAnAbility;

    public bool shotPestOnce = false;
    public bool shotVirusOnce = false;
    public bool castVolcanoOnce = false;
    public bool castEarthquakeOnce = false;
    public bool castTornadoOnce = false;

    [SerializeField] Button virusButton;
    [SerializeField] Button pestButton;
    [SerializeField] Button volcanoButton;
    [SerializeField] Button earthquakeButton;
    [SerializeField] Button tornadoButton;

    [SerializeField] GameObject earth;
    [SerializeField] Transform aimSpritePosition;
    [SerializeField] float projectileSpeed = 8f;

    [SerializeField] GameObject virusProjectile;
    [SerializeField] GameObject pestProjectile;

    [SerializeField] GameObject earthquakeAbility;
    [SerializeField] GameObject volcanoAbility;
    [SerializeField] GameObject tornadoAbility;

    Dictionary<Projectile, GameObject> projectile;
    Dictionary<Ability, GameObject> ability;

    Projectile selectedProjectile = Projectile.virus;

    Ability selectedAbility = Ability.volcano;

    enum Projectile
    {
        virus,
        pest,
        ability
    }

    public enum Ability
    {
        volcano,
        earthquake,
        tornado
    }

    private void Awake()
    {
        movement = transform.GetComponent<Movement>();
    }

    void Start()
    {
        aimPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        projectile = new Dictionary<Projectile, GameObject> {
            { Projectile.virus, virusProjectile },
            { Projectile.pest, pestProjectile }
        };

        ability = new Dictionary<Ability, GameObject> {
            {Ability.earthquake, earthquakeAbility },
            {Ability.tornado, tornadoAbility },
            {Ability.volcano, volcanoAbility }
        };

        volcanoCooldownTimer = volcanoCooldown;
        earthquakeCooldownTimer = earthquakeCooldown;
        tornadoCooldownTimer = tornadoCooldown;

    }

    
    void Update()
    {
        CountProjectilesCooldowns();
        CountAbilitiesButtonsCooldown();
        CheckChangeProjectileType();
        CheckSelectedAbility();

        aimPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = aimPosition - (Vector2)movement.currentPosition;

        aimSpritePosition.position = aimPosition;

        if (Input.GetMouseButtonDown(0) && Player.instance.canCastAbilities)
        {
            if (!selectedAnAbility)
            {
                DoShoot();
            }
            if (selectedAnAbility)
            {
                DoAbility();
                selectedAnAbility = false;
            }

        }

        CheckButtonsColors();

    }

    void CountProjectilesCooldowns()
    {
        virusShotTimer += Time.deltaTime;
        pestShotTimer += Time.deltaTime;

        canShootVirus = virusShotTimer >= virusShotCooldown;
        canShootPest = pestShotTimer >= pestShotCooldown;
    }

    void DoShoot()
    {
        if (selectedProjectile == Projectile.virus)
        {
            if (!canShootVirus)
            {
                return;
            }
            virusShotTimer = 0f;

            GameObject projInst = Instantiate(projectile[selectedProjectile], movement.currentPosition, Quaternion.identity);
            projInst.gameObject.GetComponent<Virus>().Initialize(virusDamage, virusContaminationDamage, virusContaminationRadius);

            if (projInst.transform.GetComponent<Rigidbody2D>() != null)
            {
                
                projInst.transform.GetComponent<Rigidbody2D>().velocity = aimDirection.normalized * projectileSpeed;
            }

            if (!shotVirusOnce)
            {
                shotVirusOnce = true;
            }
            //AudioManager.instance.GetComponent<AudioSource>().clip = AudioManager.instance.virus;
            AudioManager.instance.GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.virus);

        }

        if (selectedProjectile == Projectile.pest)  
        {
            if (!canShootPest)
            {
                return;
            }
            pestShotTimer = 0f;

            GameObject projInst = Instantiate(projectile[selectedProjectile], new Vector3(earth.transform.position.x, Mathf.Clamp(aimPosition.y,minYAimPosition, maxYAimPosition),0), Quaternion.identity);
            projInst.gameObject.GetComponent<Pest>().Initialize(pestDamage, pestSlow);
            if (projInst.transform.GetComponent<Rigidbody2D>() != null)
            {
                
                projInst.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed,0);
            }

            if (!shotPestOnce)
            {
                shotPestOnce = true;
            }

            AudioManager.instance.GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.pest);

        }


    }

    void CountAbilitiesButtonsCooldown()
    {
        if (!canCastVolcano) 
        {
            volcanoCooldownTimer += Time.deltaTime;
            if(volcanoCooldownTimer >= volcanoCooldown)
            {
                canCastVolcano = true;
                volcanoCooldownTimer = volcanoCooldown;
            }
        }
        if (!canCastEarthquake)
        {
            earthquakeCooldownTimer += Time.deltaTime;
            if (earthquakeCooldownTimer >= earthquakeCooldown)
            {
                canCastEarthquake = true;
                earthquakeCooldownTimer = earthquakeCooldown;
            }
        }
        if (!canCastTornado) 
        {
            tornadoCooldownTimer += Time.deltaTime;
            if (tornadoCooldownTimer >= tornadoCooldown)
            {
                canCastTornado = true;
                tornadoCooldownTimer = tornadoCooldown;
            }
        }
    }

    void DoAbility()
    {
        switch (selectedAbility)
        {
            case Ability.volcano:
                if (canCastVolcano)
                    DoVolcano();
                break;
            case Ability.earthquake:
                if (canCastEarthquake)
                    DoEarthquake();
                break;
            case Ability.tornado:
                if (canCastTornado)
                    DoTornado();
                break;
            default:
                break;
        }
    }

    private void DoTornado()
    {      
        GameObject abilityInst = Instantiate(ability[Ability.tornado], new Vector3(earth.transform.position.x, aimPosition.y, 0), Quaternion.identity);
        abilityInst.GetComponent<Rigidbody2D>().velocity = new Vector2(tornadoSpeed, 0);
        abilityInst.GetComponent<Tornado>().SetDamage(tornadoDamage); 
        abilityInst.GetComponent<Tornado>().SetCarryVelocity(tornadoCarryVelocity);

        if (!castTornadoOnce)
        {
            castTornadoOnce = true;
        }

        canCastTornado = false;
        tornadoCooldownTimer = 0f;
    }

    private void DoEarthquake()
    {       
        GameObject abilityInst = Instantiate(ability[Ability.earthquake], (Vector3)aimPosition, Quaternion.identity);
        abilityInst.GetComponent<Earthquake>().StartTimer(earthquakeTimer);
        if (!castEarthquakeOnce)
        {
            castEarthquakeOnce = true;
        }

        canCastEarthquake = false;
        earthquakeCooldownTimer = 0f;
    }

    private void DoVolcano()
    {   
        GameObject abilityInst = Instantiate(ability[Ability.volcano], (Vector3)aimPosition, Quaternion.identity);
        abilityInst.GetComponent<Volcano>().StartTimer(volcanoTimer);
        abilityInst.GetComponent<Volcano>().SetDamage(volcanoDamage);

        if (!castVolcanoOnce)
        {
            castVolcanoOnce = true;
        }

        canCastVolcano = false;
        volcanoCooldownTimer = 0f;
    }

    void CheckButtonsColors()
    {
        if (!selectedAnAbility)
        {
            switch (selectedProjectile)
            {
                case Projectile.virus:
                    pestButton.image.color = Color.white;
                    volcanoButton.image.color = Color.white;
                    earthquakeButton.image.color = Color.white;
                    tornadoButton.image.color = Color.white;
                    virusButton.image.color = new Color32(187, 255, 117, 255);
                    break;
                case Projectile.pest:
                    virusButton.image.color = Color.white;
                    volcanoButton.image.color = Color.white;
                    earthquakeButton.image.color = Color.white;
                    tornadoButton.image.color = Color.white;
                    pestButton.image.color = new Color32(187, 255, 117, 255);
                    break;
                default:
                    break;
            }
        }
        if (selectedAnAbility)
        {
            switch (selectedAbility)
            {
                case Ability.volcano:
                    volcanoButton.image.color = new Color32(187, 255, 117, 255);
                    virusButton.image.color = Color.white;
                    earthquakeButton.image.color = Color.white;
                    tornadoButton.image.color = Color.white;
                    pestButton.image.color = Color.white;
                    break;
                case Ability.earthquake:
                    earthquakeButton.image.color = new Color32(187, 255, 117, 255);
                    virusButton.image.color = Color.white;
                    volcanoButton.image.color = Color.white;
                    tornadoButton.image.color = Color.white;
                    pestButton.image.color = Color.white;
                    break;
                case Ability.tornado:
                    tornadoButton.image.color = new Color32(187, 255, 117, 255);
                    virusButton.image.color = Color.white;
                    volcanoButton.image.color = Color.white;
                    earthquakeButton.image.color = Color.white;
                    pestButton.image.color = Color.white;
                    break;
                default:
                    break;
            }
        }
    }

    void ResetButtonsColors()
    {
        virusButton.image.color = Color.white;
        pestButton.image.color = Color.white;
        volcanoButton.image.color = Color.white;
        earthquakeButton.image.color = Color.white;
        tornadoButton.image.color = Color.white;
    }

    void SelectVirusProjectile()
    {
        selectedProjectile = Projectile.virus;
        virusButton.image.color = new Color32(187, 255, 117, 255);
        Debug.Log("Selected " + selectedProjectile);
    }

    void SelectPestProjectile()
    {
        selectedProjectile = Projectile.pest;
        Debug.Log("Selected " + selectedProjectile);
    }

    void CheckChangeProjectileType()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedAnAbility = false;
            if (selectedProjectile != Projectile.pest && Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectPestProjectile();
            }

            if (selectedProjectile != Projectile.virus && Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectVirusProjectile();
            }
        }

    }

    void CheckSelectedAbility()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            //selectedAnAbility = true;

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SelectVolcano();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SelectEarthquake();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SelectTornado();
            }

            Debug.Log("Selected ability " + selectedAbility);

        }

    }

    public void SelectVolcano()
    {
        selectedAnAbility = true;
        selectedAbility = Ability.volcano;
    }

    public void SelectEarthquake()
    {
        selectedAnAbility = true;
        selectedAbility = Ability.earthquake;
    }

    public void SelectTornado()
    {
        selectedAnAbility = true;
        selectedAbility = Ability.tornado;
    }

}
