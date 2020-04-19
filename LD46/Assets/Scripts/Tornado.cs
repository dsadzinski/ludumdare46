using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{

    float damage;
    public float carryVelocity { get; private set; }

    float damageTickCooldown = 0.06f;
    float damageTickTimer;


    void Start()
    {
        
    }

    
    void Update()
    {
        damageTickTimer += Time.deltaTime;
        if(transform.position.x >= 3.5f)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetCarryVelocity(float v)
    {
        carryVelocity = v;
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
            Debug.Log("Damage Tick!");
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Carry Enemy!");
                other.gameObject.GetComponent<Enemy>().ApplyDamage(damage);
            }
        }

    }

}
