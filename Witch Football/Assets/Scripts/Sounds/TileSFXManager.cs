using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSFXManager : MonoBehaviour
{
    public AudioClip FallingRock;
    public AudioClip Shooter;
    public AudioClip SpikySlash;
    public AudioClip Explode;
    public AudioClip Rise;
    public AudioClip Fall;
    public AudioClip Invisible;
    public AudioClip Reveal;
    public AudioClip MysteryBox;
    public AudioClip PotionGlass;
    public AudioClip ObjectLanded;

    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }
    public void PlayRandom(AudioClip[] clips){
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
    public void PlaySafe(AudioClip clip){
        if(!audioSource.isPlaying && audioSource.clip != clip){
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    void StopSounds(){
        audioSource.enabled = false;
        audioSource.enabled = true;
    }
}
