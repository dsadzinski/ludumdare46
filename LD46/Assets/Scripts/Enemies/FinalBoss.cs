using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : Boss
{
    [SerializeField] Sprite finalBossSprite;

    private float baseDamage;
    private float baseMaxHealth;
    private float baseSpeed;

    public override void Initialize(int i, bool final)
    {
        spriteRenderer.sprite = finalBossSprite;
       
        baseDamage = 100f;
        baseMaxHealth = 1000f;
        baseSpeed = 0.48f;

        damage = baseDamage;
        maxHealth = baseMaxHealth * (i+1);
        speed = baseSpeed;
        spawnedWithSpeed = speed;

        currentHealth = maxHealth;
    }
}
