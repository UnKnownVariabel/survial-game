using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] randomClips;
    [SerializeField] private AudioClip[] triggerdClips;
    [SerializeField] private float timeBetweenClips;
    [SerializeField] private float randomOffset;
    
    private IEnumerator coroutine;

    public void PlayClip(int i)
    {
        StopCoroutine(coroutine);
        coroutine = PlayClipRoutine(i);
        StartCoroutine(coroutine);
    }

    private void Start()
    {
        coroutine = PlayRandom();
        StartCoroutine(coroutine);
    }

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

    IEnumerator PlayClipRoutine(int i)
    {
        audioSource.clip = triggerdClips[i];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        coroutine = PlayRandom();
        StartCoroutine(coroutine);
    }
}
