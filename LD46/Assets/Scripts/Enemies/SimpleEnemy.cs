using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    private float baseDamage = 2f;
    private float baseMaxHealth = 10f;
    private float baseSpeed = 0.3f;

    [SerializeField] List<Sprite> spritesLevel1 = new List<Sprite>();
    [SerializeField] List<Sprite> spritesLevel2 = new List<Sprite>();
    [SerializeField] List<Sprite> spritesLevel3 = new List<Sprite>();

    private List<Sprite>[] spriteLists;

    public void Initialize(int gameLevel, float oilLevel)
    {
        int wave = GameManager.instance.waveNumber;
        
        spriteLists = new List<Sprite>[]{ spritesLevel1, spritesLevel2, spritesLevel3 };

        int spriteIndex = Mathf.Clamp(Mathf.CeilToInt(gameLevel / 3) - 1, 0, 2);

        spriteRenderer.sprite = spriteLists[spriteIndex][Random.Range(0,3)];

        damage = baseDamage * gameLevel + oilLevel * 0.3f;
        maxHealth = baseMaxHealth + (gameLevel*8) + oilLevel * 0.4f;
        speed = baseSpeed + (baseSpeed*((gameLevel* (gameLevel*0.6f)) / 2)) + (wave*0.07f);
        spawnedWithSpeed = speed;
        currentHealth = maxHealth;

        //Debug.Log("enemy Speed = " + speed);

    }

    
    void Start()
    {
        rigidbody.velocity = new Vector2(-speed, 0);
    }

    public override void Update()
    {
        base.Update();
        
        if (isSlowed)
        {
            slowTimer += Time.deltaTime;
            if (slowTimer >= slowCooldown)
            {
                RemoveSlow();
            }
        }

        rigidbody.velocity = isSlowed ? new Vector2(-slowedSpeed, 0) : new Vector2(-speed, 0);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        //Debug.Log("Enemy Dies!");
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //TODO: If other == terra grande -> Do damage to player
    }
}
