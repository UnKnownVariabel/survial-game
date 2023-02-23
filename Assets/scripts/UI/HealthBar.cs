using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Animation barAnimation;
    public void SetHealth(float health)
    {
        if(health > 1 || health < 0)
        {
            Debug.Log("health bar is to be sett betwen 0 and 1 " + health.ToString() + "is incorect input");
        }
        slider.value = health;
        barAnimation.Play();
    }
}
