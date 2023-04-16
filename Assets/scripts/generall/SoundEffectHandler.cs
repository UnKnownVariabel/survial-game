using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SoundEffects handler plays sound effects and is atached to a destructibleObject.
public class SoundEffectHandler : MonoBehaviour
{
    public static List<SoundEffectHandler> instances;

    // Setter and getter to the volume on the audioSource.
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
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] randomClips;
    [SerializeField] private AudioClip[] triggerdClips;
    [SerializeField] private float timeBetweenClips;
    [SerializeField] private float randomOffset;
    
    private IEnumerator coroutine;

    // PlayClip plays clip specified by its index in the clips array.
    public void PlayClip(int i)
    {
        StopCoroutine(coroutine);
        coroutine = PlayClipRoutine(i);
        StartCoroutine(coroutine);
    }

    // Start is called before the first frame update.
    private void Start()
    {
        if(instances == null)
        {
            instances = new List<SoundEffectHandler>();
        }
        instances.Add(this);
        volume = PlayerPrefs.GetFloat("effects volume");
        coroutine = PlayRandom();
        StartCoroutine(coroutine);
    }

    // OnDestroy is called when gameObject is destroyed.
    private void OnDestroy()
    {
        instances.Remove(this);
    }

    // PlayRandom plays a random clip out of the random clips array and then whaits a while and calls itself again.
    IEnumerator PlayRandom()
    {
        if(randomClips.Length > 0)
        {
            yield return new WaitForSeconds(timeBetweenClips + Random.Range(0, randomOffset));
            audioSource.clip = randomClips[Random.Range(0, randomClips.Length)];
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            StartCoroutine(coroutine);
        }
        else
        {
            yield break;
        }
    }

    // PlayCLipRoutine is the coroutine that actualy plays the specified clip in PlayClip.
    IEnumerator PlayClipRoutine(int i)
    {
        audioSource.clip = triggerdClips[i];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        coroutine = PlayRandom();
        StartCoroutine(coroutine);
    }
}
