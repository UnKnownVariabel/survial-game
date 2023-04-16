using UnityEngine;
using UnityEngine.UI;

// Healtbar is attached to all healthbars and is used to adjust the healthbar.
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Animation barAnimation;
    [SerializeField] private bool alwaysVisible;
    [SerializeField] private Image background;
    [SerializeField] private Image border;

    // Sets the value of the healthbar slider and starts animation.
    public void SetHealth(float health)
    {
        if(health > 1 || health < 0)
        {
            Debug.Log("health bar is to be sett betwen 0 and 1 " + health.ToString() + "is incorect input");
        }
        slider.value = health;
        if (!alwaysVisible)
        {
            barAnimation.Rewind();
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
