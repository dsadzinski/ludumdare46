using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{

    float damage;
    float contaminationDamage;
    float contaminationRadius;

    public void Initialize(float d, float contD, float r)
    {
        damage = d;
        contaminationDamage = contD;
        contaminationRadius = r;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= 4)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().ApplyDamage(damage);
            other.gameObject.GetComponent<Enemy>().ApplyContamination(contaminationRadius, contaminationDamage);
            Destroy(this.gameObject);
        }
    }

}
