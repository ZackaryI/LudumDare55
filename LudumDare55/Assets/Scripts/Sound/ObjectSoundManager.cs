using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectSoundManager : MonoBehaviour
{
    [Header("Conditions")]
    public bool isFootstepsOn = false;

    [Header("Soundclips")]
    public AudioClip footsteps;
    public AudioClip deathSound;
    public AudioClip hitSound;
    public AudioClip attackSound;
    private AudioSource audioSource; 
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void playAudioDeath()
    {
        audioSource.PlayOneShot(deathSound);
    }
    
    void playAudioHit()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
    void playAudioAttack()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(attackSound);

        }
    }
}
