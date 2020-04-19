using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trex : MonoBehaviour
{

    [SerializeField] float minX;
    [SerializeField] float minY;
    [SerializeField] float maxX;
    [SerializeField] float maxY;

    float health = 100f;

    float stopTime = 0.25f;
    float stopTimer;

    Vector2 speed;

    int directionX;
    int directionY;

    void Start()
    {
        Player.instance.TrexAlive = true;
        Spawn();
    }

    void Update()
    {
        stopTimer += Time.deltaTime;
        Move();
    }

    void StopMovement()
    {
        stopTimer = 0f;
    }

    private void Move()
    {
        if(transform.position.y <= minY)
        {
            directionY = 1;
            RandomizeSpeed();
        }
        if(transform.position.y >= maxY)
        {
            directionY = -1;
            RandomizeSpeed();
        }
        if(transform.position.x <= minX)
        {
            directionX = 1;
            RandomizeSpeed();
        }
        if(transform.position.x >= maxX)
        {
            directionX = -1;
            RandomizeSpeed();
        }

        if (stopTimer >= stopTime)
        {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(speed.x * directionX, speed.y * directionY);
        }
        else
        {
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

    }

    private void RandomizeSpeed()
    {
        speed = new Vector2(Random.Range(0.3f, 0.6f), Random.Range(0.1f, 0.3f));
    }

    void Spawn()
    {
        stopTimer = stopTime;
        directionY = 1;
        directionX = 1;
        transform.position = new Vector3(0, 0.8f);
        RandomizeSpeed();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(other.gameObject.GetComponent<Boss>() != null)
            {
                other.gameObject.GetComponent<Boss>().ApplyDamage(300);
            }
            else
            {
                Destroy(other.gameObject);
            }
            health -= 5;
            if(health <= 0)
            {
                Player.instance.TrexAlive = false;
                OilManager.instance.IncreaseOilLevel(40f);
                Destroy(this.gameObject);
            }
            StopMovement();
        }
    }

}
