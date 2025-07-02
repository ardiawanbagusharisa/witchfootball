using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISFXManager : MonoBehaviour
{
    public AudioClip StartOrPause;
    public AudioClip OK;
    public AudioClip Back;
    public AudioClip Earthquake;

    private AudioSource audioSource;
    
    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }
    public void PlayRandom(AudioClip[] clips){
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
    void StopSounds(){
        audioSource.enabled = false;
        audioSource.enabled = true;
    }
}
