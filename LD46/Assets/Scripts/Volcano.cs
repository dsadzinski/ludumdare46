using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : TimedAbility
{

    [SerializeField] new CircleCollider2D collider;
    float damage;

    float maxRadius = 0.45f;
    float currentRadius = 0;
    float radiusTimer = 0f;
    float timeToMaxRadius = 0.25f;

    float damageTickCooldown = 0.15f;
    float damageTickTimer;

    void Start()
    {
        collider = transform.GetComponent<CircleCollider2D>();
    }

    
    public override void Update()
    {
        base.Update();

        damageTickTimer += Time.deltaTime;
        SplashLava();
    }

    void SplashLava()
    {
        if (!startedTimer)
        {
            currentRadius = 0f;
            radiusTimer = 0f;
            collider.radius = currentRadius;
            return;
        }

        radiusTimer += Time.deltaTime;
        currentRadius = maxRadius * Mathf.Clamp(radiusTimer / timeToMaxRadius, 0, 1);
        collider.radius = currentRadius;
    }

    public void SetDamage(float d)
    {
        damage = d;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (damageTickTimer >= damageTickCooldown)
        {
            damageTickTimer = 0f;

            if (other.CompareTag("Enemy"))
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("Apply Damage " + damage);
                    other.gameObject.GetComponent<Enemy>().ApplyDamage(damage);
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }

}
