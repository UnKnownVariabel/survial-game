using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Animation barAnimation;
    [SerializeField] private bool alwaysVisible;
    [SerializeField] private Image background;
    [SerializeField] private Image border;
    public void SetHealth(float health)
    {
        if(health > 1 || health < 0)
        {
            Debug.Log("health bar is to be sett betwen 0 and 1 " + health.ToString() + "is incorect input");
        }
        slider.value = health;
        if (!alwaysVisible)
        {
            barAnimation.Play();
        }
        else if(health < 1)
        {
            background.color = new Color(background.color.r, background.color.g, background.color.b, 1f);
            border.color = new Color(border.color.r, border.color.g, border.color.b, 1f);
        }
        else
        {
            background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
            border.color = new Color(border.color.r, border.color.g, border.color.b, 0f);
        }

    }
}
