using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] protected GameObject bar;
    protected Vector2 fullBarPosition;
    protected Vector2 emptyBarPosition;

    protected Vector2 currentBarPosition;

    protected Player player;
    protected OilManager gameManager;

    private void Awake()
    {
        fullBarPosition = bar.transform.position;
        emptyBarPosition = new Vector2(fullBarPosition.x - 1.12f, fullBarPosition.y);
    }

    void Start()
    {
        player = Player.instance;
        gameManager = OilManager.instance;
        currentBarPosition = fullBarPosition;
        bar.transform.position = currentBarPosition;
    }


    void Update()
    {
        CalculateCurrentPosition();
        bar.transform.position = currentBarPosition;
    }

    public virtual void CalculateCurrentPosition()
    {
        float HealthOffsetX = fullBarPosition.x - emptyBarPosition.x;
        float currentHealthX = ((player.currentHealth / player.maxHealth) * HealthOffsetX) + emptyBarPosition.x;

        currentBarPosition = new Vector2(currentHealthX, fullBarPosition.y);
    }
}
