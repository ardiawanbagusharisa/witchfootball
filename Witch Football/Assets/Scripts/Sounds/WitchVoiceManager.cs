using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchVoiceManager : MonoBehaviour
{
    //Activity
    public AudioClip Shooting;
    public AudioClip Passing;
    public AudioClip Tackling;
    public AudioClip Controlling;
    public AudioClip Scoring;
    public AudioClip Falling;
    public AudioClip Tackled;

    //Expression
    public AudioClip Happy;
    public AudioClip Sad;
    public AudioClip Laugh;
    public AudioClip Cry;
    public AudioClip Boring;

    //Skills
    public AudioClip LightDamage;
    public AudioClip HeavyDamage;
    public AudioClip LightHeal;
    public AudioClip HeavyHeal;
    public AudioClip LightPower;
    public AudioClip HeavyPower;
    public AudioClip LightSpeed;
    public AudioClip HeavySpeed;
    public AudioClip DamageUp;
    public AudioClip HealUp;
    public AudioClip PowerUp;
    public AudioClip SpeedUp;

    private AudioSource audioSource;
    
    //Play, SetVolume
    //Voice Categories: 
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void VoicePlay(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }
    public void VoicePlayChance(AudioClip clip, int chance = 50){
        if(Random.Range(0, 100) <= chance+1){
            audioSource.PlayOneShot(clip);
        }
    }
    public void VoicePlayRandom(AudioClip[] clips){
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
    public void PlayMagicVoice(MagicSkill magic){
        // Skill SFX based on type
        if(magic.category == MagicSkill.Category.Light){
            if(magic.type == MagicSkill.Type.Speed){
                VoicePlay(LightSpeed);    
            } else if(magic.type == MagicSkill.Type.Damage){
                VoicePlay(LightDamage);    
            } else if(magic.type == MagicSkill.Type.Power){
                VoicePlay(LightPower);    
            } else if(magic.type == MagicSkill.Type.HealthPoint){
                VoicePlay(LightHeal);    
            }
        } else if(magic.category == MagicSkill.Category.Heavy){
            if(magic.type == MagicSkill.Type.Speed){
                VoicePlay(HeavySpeed);    
            } else if(magic.type == MagicSkill.Type.Damage){
                VoicePlay(HeavyDamage);    
            } else if(magic.type == MagicSkill.Type.Power){
                VoicePlay(HeavyPower);    
            } else if(magic.type == MagicSkill.Type.HealthPoint){
                VoicePlay(HeavyHeal);    
            }
        }
    }
    public void PlayMysteryBoxVoice(MagicSkill.Type type){
        // Effect Voice based on type
        if(type == MagicSkill.Type.Speed){
            VoicePlay(SpeedUp);    
        } else if(type == MagicSkill.Type.Damage){
            VoicePlay(DamageUp);    
        } else if(type == MagicSkill.Type.Power){
            VoicePlay(PowerUp);    
        } else if(type == MagicSkill.Type.HealthPoint){
            VoicePlay(HealUp);    
        }
    }
    void StopVoices(){
        audioSource.enabled = false;
        audioSource.enabled = true;
    }
}
