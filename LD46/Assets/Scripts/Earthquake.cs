using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : TimedAbility
{

    void Start()
    {
        
    }
  
    public override void Update()
    {
        base.Update();

    }


}

public abstract class TimedAbility : MonoBehaviour
{
    public float timer { get; protected set; } = 0f;
    public bool startedTimer = false;

    public virtual void Update()
    {
        timer -= Time.deltaTime;
        if (startedTimer && timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void StartTimer(float t)
    {
        if (startedTimer == false)
        {
            Debug.Log("Started Timer :" + t);
            timer = t;
            startedTimer = true;
        }
    }
}
