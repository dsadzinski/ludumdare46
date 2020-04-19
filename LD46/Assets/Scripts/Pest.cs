using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pest : MonoBehaviour
{
    float damage;
    float slowAmount;

    public void Initialize(float d, float s)
    {
        damage = d;
        slowAmount = s;
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        if(transform.position.x >= 4)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy")){
            other.gameObject.GetComponent<Enemy>().ApplyDamage(damage);
            other.gameObject.GetComponent<Enemy>().ApplySlow(slowAmount);
        }
    }
}
