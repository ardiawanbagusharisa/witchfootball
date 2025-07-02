using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchSFXManager : MonoBehaviour
{
    // Activity
    public AudioClip Shooting;
    public AudioClip Passing;
    public AudioClip Tackling;
    public AudioClip Tackled;
    public AudioClip Stunned;
    public AudioClip Falling;
    public AudioClip Jumping;
    public AudioClip Exploding;

    //Skills
    public AudioClip LightDamage;
    public AudioClip HeavyDamage;
    public AudioClip LightHeal;
    public AudioClip HeavyHeal;
    public AudioClip LightPower;
    public AudioClip HeavyPower;
    public AudioClip LightSpeed;
    public AudioClip HeavySpeed;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }
    public void PlayMagicSFX(MagicSkill magic){
        // Skill SFX based on type
        if(magic.category == MagicSkill.Category.Light){
            if(magic.type == MagicSkill.Type.Speed){
                Play(LightSpeed);    
            } else if(magic.type == MagicSkill.Type.Damage){
                Play(LightDamage);    
            } else if(magic.type == MagicSkill.Type.Power){
                Play(LightPower);    
            } else if(magic.type == MagicSkill.Type.HealthPoint){
                Play(LightHeal);    
            }
        } else if(magic.category == MagicSkill.Category.Heavy){
            if(magic.type == MagicSkill.Type.Speed){
                Play(HeavySpeed);    
            } else if(magic.type == MagicSkill.Type.Damage){
                Play(HeavyDamage);    
            } else if(magic.type == MagicSkill.Type.Power){
                Play(HeavyPower);    
            } else if(magic.type == MagicSkill.Type.HealthPoint){
                Play(HeavyHeal);    
            }
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
