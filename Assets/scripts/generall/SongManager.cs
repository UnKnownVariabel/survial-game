using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public float volume
    {
        get
        {
            return Audio.volume;
        }
        set 
        {
            Audio.volume = value; 
        }
    }

    [SerializeField] private AudioSource Audio;
    [SerializeField] private AudioClip[] songs;
    private int i = 0;

    void Start()
    {
        Audio.loop = true;
        StartCoroutine(PlaySong());
    }

    IEnumerator PlaySong()
    {
        Audio.clip = songs[i];
        i++;
        if(i >= songs.Length)
        {
            i = 0;
        }
        Audio.Play();
        yield return new WaitForSeconds(Audio.clip.length);
        StartCoroutine(PlaySong());
    }
}
