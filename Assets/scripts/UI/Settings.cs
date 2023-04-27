using UnityEngine;
using UnityEngine.UI;

// Settings is attached to the settings menu.
public class Settings : MonoBehaviour
{
    // Reades and writes to the auto save preffrence.
    public static bool autoSave
    {
        get
        {
            return PlayerPrefs.GetInt("auto save") != 0;
        }
        set
        {
            PlayerPrefs.SetInt("auto save", value ? 1 : 0);
            Debug.Log(PlayerPrefs.GetInt("auto save"));
        }
    }

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;
    [SerializeField] private Toggle autoSaveToggle;

    // Awake is called when script instance is loaded.
    private void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat("music volume");
        effectSlider.value = PlayerPrefs.GetFloat("effects volume");
        autoSaveToggle.isOn = autoSave;
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

    // Sets auto save.
    public void SetAutoSave(Toggle autoSave)
    {
        Settings.autoSave = autoSave.isOn;
    }
}
