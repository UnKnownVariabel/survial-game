using System.Collections;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public float volume
    {
        get
        {
            return audioSource.volume;
        }
        set 
        {
            audioSource.volume = value; 
        }
    }

    private int _mood = -1;
    public int mood
    {
        get
        {
            return _mood;
        }
        set
        {
            if (value != _mood)
            {
                StopCoroutine(coroutine);
                switch(value)
                {
                    case 0:
                        songs = daySongs;
                        break;
                    case 1:
                        songs = nightSongs;
                        break;  
                }
                i = Random.Range(0, songs.Length);
                StartCoroutine(coroutine);
            }
            _mood = value;
        }
    }
    public static SongManager instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] daySongs;
    [SerializeField] private AudioClip[] nightSongs;
    private AudioClip[] songs;

    private int i = 0;
    private IEnumerator coroutine;

    void Start()
    {
        instance = this;
        coroutine = PlaySong();
        audioSource.loop = true;
        //StartCoroutine(coroutine);
        mood = 0;
        volume = PlayerPrefs.GetFloat("music volume");
    }

    IEnumerator PlaySong()
    {
        audioSource.clip = songs[i];
        i++;
        if(i >= songs.Length)
        {
            i = 0;
        }
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        StartCoroutine(coroutine);
    }
}
