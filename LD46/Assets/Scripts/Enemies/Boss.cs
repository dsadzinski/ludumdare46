using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{

    [SerializeField] List<Sprite> possibleSprites = new List<Sprite>();

    private float baseDamage = 10f;
    private float baseMaxHealth = 200f;
    private float baseSpeed = 0.25f;

    public virtual void Initialize(int i, bool final)
    {
        if (i <= 2)
        {
            spriteRenderer.sprite = possibleSprites[i-1];
        }else if(final)
        {
            spriteRenderer.sprite = possibleSprites[2];
            baseDamage = 100f;
            baseMaxHealth = 500f;
            baseSpeed = 0.32f;
        }
        else
        {
            spriteRenderer.sprite = possibleSprites[Random.Range(0,2)];
        }

        damage = baseDamage * i;
        maxHealth = baseMaxHealth * i;
        speed = baseSpeed + baseSpeed * (i / 10);
        spawnedWithSpeed = speed;
        
        currentHealth = maxHealth;

    }

    
    void Start()
    {
        
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

        rigidbody.velocity = isSlowed? new Vector2(-slowedSpeed, 0) : new Vector2(-speed,0);

        if(currentHealth <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //TODO: If other == terra grande -> Do damage to player
    }

}
