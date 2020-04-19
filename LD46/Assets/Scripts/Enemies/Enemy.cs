using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected CircleCollider2D contaminationCol;
    [SerializeField] protected Rigidbody2D rigidbody;

    protected float damage;
    protected float maxHealth;
    protected float currentHealth;
    protected float speed = 0;

    float damageTimer;
    float damageCooldown = 1f;

    protected bool isBeingCarried = false;
    protected Vector2 carrySpeed;

    protected bool isSlowed = false;
    protected bool isContamined = false;

    protected bool createdContaminationCollider = false;

    protected float contaminationSpreadRadius;
    protected float contaminationDamage;
    protected float contaminationCooldown = 2f;
    protected float contaminationTimer;
    protected float contaminationTick = 0.5f;
    protected float contaminationTickTimer;

    protected float slowedSpeed;
    protected float slowCooldown = 3f;
    protected float slowTimer;

    protected float spawnedWithSpeed;
    protected float normalSlowSpeed;

    protected Color slowedColor = new Color32(255,200,20,255);
    protected Color contaminatedColor = new Color32(141, 255, 134, 255);

    void Start()
    {

    }

    
    public virtual void Update()
    {
        CheckContamination();
        damageTimer += Time.deltaTime;

        if(transform.position.x > 4)
        {
            transform.position = new Vector2(4, transform.position.y);
        }

    }

    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;
    }

    public void ApplySlow(float amount)
    {
        slowedSpeed = speed * amount;
        slowTimer = 0f;
        isSlowed = true;

        if (isContamined)
        {
            spriteRenderer.color = (slowedColor + contaminatedColor) / 2;
        }
        else
        {
            spriteRenderer.color = slowedColor;
        }

    }

    public void RemoveSlow()
    {
        isSlowed = false;
    }

    public void ApplyContamination(float radius, float d)
    {
        isContamined = true;
        contaminationSpreadRadius = radius;
        contaminationDamage = d;
        contaminationTimer = 0f;
        contaminationTickTimer = 0f;

        if (isSlowed)
        {
            spriteRenderer.color = (slowedColor + contaminatedColor) / 2;
        }
        else
        {
            spriteRenderer.color = contaminatedColor;
        }
    }

    public void RemoveContamination()
    {
        isContamined = false;
        contaminationDamage = 0f;
        contaminationTimer = 0f;
        contaminationTickTimer = 0f;

        spriteRenderer.color = Color.white;

    }

    public void CheckContamination()
    {
        if (isContamined)
        {
            contaminationTimer += Time.deltaTime;
            contaminationTickTimer += Time.deltaTime;

            if(contaminationTickTimer >= contaminationTick)
            {
                ApplyDamage(contaminationDamage);
            }

            if(contaminationTimer >= contaminationCooldown)
            {
                RemoveContamination();
            }

            var hit = Physics2D.CircleCast(transform.position, contaminationSpreadRadius, Vector2.up,0f);
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<Enemy>().ApplyContamination(contaminationSpreadRadius, contaminationDamage);
            }
        }
    }

    public void SetCarried(bool b)
    {
        isBeingCarried = b;
    }

    public void ApplyCarrySpeed(float v)
    {
        carrySpeed = new Vector2(v, 0);

        speed = speed - carrySpeed.x;
        if (isSlowed)
        {
            normalSlowSpeed = slowedSpeed;
            slowedSpeed -= carrySpeed.x;
        }
    }

    public void ResetSpeed()
    {
        speed = spawnedWithSpeed;
        slowedSpeed = normalSlowSpeed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tornado"))
        {
            ApplyCarrySpeed(other.gameObject.GetComponent<Tornado>().carryVelocity);
            SetCarried(true);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Earth"))
        {
            if(GameManager.instance.gameState == GameManager.GameState.game)
            DamageEarth();
        }
    }

    void DamageEarth()
    {
        if(damageTimer >= damageCooldown)
        {
            damageTimer = 0;
            UpgradesManager.instance.ChopTree(1);
            Player.instance.ApplyDamage(damage);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tornado"))
        {
            SetCarried(false);
            ResetSpeed();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, contaminationSpreadRadius);
    }

}
