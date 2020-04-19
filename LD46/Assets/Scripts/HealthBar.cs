using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : Bar
{
    
    public override void CalculateCurrentPosition()
    {
        float HealthOffsetX = fullBarPosition.x - emptyBarPosition.x;
        float currentHealthX = ((player.currentHealth / player.maxHealth) * HealthOffsetX) + emptyBarPosition.x;

        currentBarPosition = new Vector2 (currentHealthX, fullBarPosition.y);
    }

}

