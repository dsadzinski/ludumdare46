using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownImage : MonoBehaviour
{
    Shoot shoot;
    Player player;

    [SerializeField] Shoot.Ability ability;
    [SerializeField] Player.SpecialAbility specialAbility;
    Image image;

    [SerializeField] bool isSpecialAbility;

    void Start()
    {
        player = Player.instance;
        shoot = Player.instance.shoot;
        image = transform.GetComponent<Image>();
    }

    
    void Update()
    {
        if (isSpecialAbility)
        {
            switch (specialAbility)
            {
                case Player.SpecialAbility.globalEarthquakes:
                    image.fillAmount = 1 - player.globalEarthquakeCooldownTimer / player.globalEarthquakeCooldown;
                    break;
                case Player.SpecialAbility.TRex:
                    image.fillAmount = 1 - player.TrexCooldownTimer / player.TrexCooldown;
                    break;
                case Player.SpecialAbility.ET:
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (ability)
            {
                case Shoot.Ability.volcano:
                    image.fillAmount = 1 - shoot.volcanoCooldownTimer / shoot.volcanoCooldown;
                    break;
                case Shoot.Ability.earthquake:
                    image.fillAmount = 1 - shoot.earthquakeCooldownTimer / shoot.earthquakeCooldown;
                    break;
                case Shoot.Ability.tornado:
                    image.fillAmount = 1 - shoot.tornadoCooldownTimer / shoot.tornadoCooldown;
                    break;
            }
        }
    }
}
