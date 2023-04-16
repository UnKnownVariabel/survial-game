using System.Collections;
using UnityEngine;

// Songmanager plays music.
public class SongManager : MonoBehaviour
{
    // Static setter and getter for the volume of the audioSource
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

    // Mood decides which song list plays in the background.
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

    // Start is called before the first frame update.
    void Start()
    {
        instance = this;
        coroutine = PlaySong();
        audioSource.loop = true;
        mood = 0;
        volume = PlayerPrefs.GetFloat("music volume");
    }

    // PlaySong choses the next song than plays that one to the end only to than call itself again.
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
