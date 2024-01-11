using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] audioSources;
    public AudioSource dialogueSource;
    private AudioSource musicSource;
    private AudioSource soundEffectSource;
    private const float defaultVolume = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        audioSources = FindObjectsOfType<AudioSource>();
        dialogueSource = audioSources[0];
        musicSource = audioSources[1];
        soundEffectSource = audioSources[2];
    }

    /*Fade in or out the background music to a targeted volume and duration*/
    public IEnumerator fadeAudio( float duration, float targetVol = defaultVolume)
    {
        //fade in or out the current music
        float currTime = 0f;
        float startVol = musicSource.volume;
        while (currTime < duration)
        {
            currTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVol, targetVol, currTime/duration);
            yield return null;
        }
        yield break;
    }

    /*Pass audio clip to be played as background music*/
    public void PlayMusic(AudioClip clip, float volume)
    {
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlaySoundEffect(AudioClip clip, float volume, bool loop = false)
    {
        soundEffectSource.clip = clip;
        soundEffectSource.loop = loop;
        soundEffectSource.volume = volume;
        soundEffectSource.Play();
    }

    public void StopSoundEffect()
    {
        soundEffectSource.Stop();
    }
    
}


//need to check in update if we have elft cabin. If so, start music 
