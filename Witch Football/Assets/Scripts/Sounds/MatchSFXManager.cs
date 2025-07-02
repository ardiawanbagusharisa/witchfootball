using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSFXManager : MonoBehaviour
{
    public AudioClip Whistle;
    public AudioClip MysteryBox;

    public AudioSource audioSource;

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
    public void PlayTemp(AudioClip clip, Vector3 pos){
        AudioSource.PlayClipAtPoint(clip, pos);
    }
    void StopSounds(){
        audioSource.enabled = false;
        audioSource.enabled = true;
    }
}
