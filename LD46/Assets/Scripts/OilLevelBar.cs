using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilLevelBar : Bar
{

    public override void CalculateCurrentPosition()
    {
        float OffsetX = fullBarPosition.x - emptyBarPosition.x;
        float currentX = ((gameManager.oilLevel / gameManager.maxOil) * OffsetX) + emptyBarPosition.x;

        currentBarPosition = new Vector2(currentX, fullBarPosition.y);
    }
}
