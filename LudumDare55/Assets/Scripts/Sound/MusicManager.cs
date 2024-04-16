using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public float fadeDuration = 0.5f;
    [Header("Volumes")]
    [Range(0.0f, 1.0f)]
    public float volumeCombatMusic = 1f;
    [Range(0.0f, 1.0f)]
    public float volumeNoncombatMusic = 1f;
    public AudioClip combatMusic;
    public AudioClip[] nonCombatMusic;
    private AudioSource musicAudioSource;

    private void Start()
    {
        musicAudioSource = GetComponent<AudioSource>(); 
    }
    public void playCombatMusic()
    {
        musicAudioSource.volume = 0;
        musicAudioSource.clip = combatMusic;
        StartCoroutine(FadeAudioSource(musicAudioSource, volumeCombatMusic, fadeDuration)); //USAGE HERE
    }

    public void playNonCombatMusic()
    {
        musicAudioSource.volume = 0; 
        musicAudioSource.clip = nonCombatMusic[Random.Range(0, nonCombatMusic.Length - 1)];
        StartCoroutine(FadeAudioSource(musicAudioSource, 1, fadeDuration)); //USAGE HERE


    }


    public IEnumerator FadeAudioSource(AudioSource audioSource, float targetVolume, float fadeDuration, bool destroy = false)
    {
        float startVolume = audioSource.volume;
        float timer = 0;

        while (audioSource.volume != targetVolume)
        {
            timer += Time.unscaledDeltaTime;

            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / fadeDuration);

            yield return null;
        }

        audioSource.Play();
        if (audioSource.volume <= 0)
            audioSource.Stop();
         
    }

}
