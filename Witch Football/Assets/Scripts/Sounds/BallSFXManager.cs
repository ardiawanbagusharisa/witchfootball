using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSFXManager : MonoBehaviour
{
    public AudioClip Kicked;
    public AudioClip Controlled;
    public AudioClip Falling;
    public AudioClip Goal;
    public AudioClip HitingPole;
    public AudioClip Raged;
    public AudioClip Release;
    public AudioClip BallBounce;
    public AudioClip DribbleNormal;
    public AudioClip DribbleSpring;
    public AudioClip DribbleSummer;
    public AudioClip DribbleAutumn;
    public AudioClip DribbleWinter;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }
    // <Edit later> Use it for seasonal tiles
    public void PlayDribbleSFX(Tile.SeasonType type){
        // Tile SFX based on type
        if(type == Tile.SeasonType.None){
            audioSource.PlayOneShot(DribbleNormal);
        } else if(type == Tile.SeasonType.Spring){
            audioSource.PlayOneShot(DribbleSpring);
        } else if(type == Tile.SeasonType.Summer){
            audioSource.PlayOneShot(DribbleSummer);
        } else if(type == Tile.SeasonType.Autumn){
            audioSource.PlayOneShot(DribbleAutumn);
        } else if(type == Tile.SeasonType.Winter){
            audioSource.PlayOneShot(DribbleWinter);
        }
    }
    public void PlayRandom(AudioClip[] clips){
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
    public void StopSounds(){
        audioSource.enabled = false;
        audioSource.enabled = true;
    }
}
