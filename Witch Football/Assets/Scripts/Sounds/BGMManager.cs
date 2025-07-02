using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] BaseSoundtracks;
    public AudioClip[] SpringSoundtracks;
    public AudioClip[] AutumnSoundtracks;
    public AudioClip[] WinterSoundtracks;
    public AudioClip[] SummerSoundtracks;
    
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        audioSource = GetComponent<AudioSource>();
        if(!audioSource.playOnAwake){
            PlayRandom(BaseSoundtracks);
        }
    }

    void LateUpdate() {
        if(!audioSource.isPlaying){
            //PlayRandom(BaseSoundtracks);
            PlayRandomAll();
        }
    }

    void PlayRandom(AudioClip[] clips){
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }

    void PlayRandomAll(){
        int r = Random.Range(0, 5);
        if(r == 0) PlayRandom(BaseSoundtracks);
        if(r == 1) PlayRandom(SpringSoundtracks);
        if(r == 2) PlayRandom(AutumnSoundtracks);
        if(r == 3) PlayRandom(WinterSoundtracks);
        if(r == 4) PlayRandom(SummerSoundtracks);
    }
}
