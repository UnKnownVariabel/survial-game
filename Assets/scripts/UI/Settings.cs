using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;
    private void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat("music volume");
        effectSlider.value = PlayerPrefs.GetFloat("effects volume");
    }

    public void ChangeMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("music volume", volume);
        if(SongManager.instance != null)
        {
            SongManager.instance.volume = volume;
        }
    }

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
