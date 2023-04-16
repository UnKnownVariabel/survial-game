using UnityEngine;
using UnityEngine.UI;

// Settings is attached to the settings menu.
public class Settings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;

    // Awake is called when script instance is loaded.
    private void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat("music volume");
        effectSlider.value = PlayerPrefs.GetFloat("effects volume");
    }

    // Changes Music volume and is called from music slider.
    public void ChangeMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("music volume", volume);
        if(SongManager.instance != null)
        {
            SongManager.instance.volume = volume;
        }
    }

    // Changes effects volume and is called from sound effects slider.
    public void ChangeEffectsVolume(float volume)
    {
        PlayerPrefs.SetFloat("effects volume", volume);
        if(SoundEffectHandler.instances != null)
        {
            foreach(SoundEffectHandler instance in SoundEffectHandler.instances)
            {
                instance.volume = volume;
            }
        }
    }
}
