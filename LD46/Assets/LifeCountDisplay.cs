using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCountDisplay : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer spriteRenderer;
    int spriteIndex;
    
    void Update()
    {
        spriteIndex = Player.instance.currentLifes < 0 ? 0 : Player.instance.currentLifes;
        spriteRenderer.sprite = sprites[spriteIndex];
    }
}
