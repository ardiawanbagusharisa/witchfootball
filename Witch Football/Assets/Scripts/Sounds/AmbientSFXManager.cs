using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSFXManager : MonoBehaviour
{
    public enum Seasons{
        Normal, 
        Spring, 
        Summer,
        Fall, 
        Winter
    };
    public Seasons Season;
    public enum Weathers{
        Normal, 
        Windy,
        Rain,
        Storm
    };
    public Weathers Weather;
    public enum DayNightTime{
        Normal, 
        Day, 
        Night
    };
    public DayNightTime DayNight;

    // Seasons
    public AudioClip Lava;
    public AudioClip Snow;
    public AudioClip Snowing;
    public AudioClip SteamUp;

    // Weather
    public AudioClip Wind;
    public AudioClip Storm;
    public AudioClip Rain;
    public AudioClip HeavyThunder;
    public AudioClip RainAndThunder;
    public AudioClip ThunderStorm;

    // Day & Night
    public AudioClip MorningBird;
    public AudioClip Ghostly;
    public AudioClip Night;
    public AudioClip Sunny;
    public AudioClip NightFrog;
    
    // Other
    public AudioClip Frogs;
    public AudioClip Crickets;
    public AudioClip BirdsCricketsDogs;
    public AudioClip Wolves;
    public AudioClip Raven;
    public AudioClip Bird;

    private AudioSource audioSource;
    private AudioSource audioSeason;
    private AudioSource audioWeather;
    private AudioSource audioDayNight;
    
    public float minTimeBetween = 5f;
    public float maxTimeBetween = 20f;
    private float TimeBetween   = 10f;

    public float minSeasonTimer = 5f;
    public float maxSeasonTimer = 20f;
    private float seasonTimer = 10f; 
    public float minWeatherTimer = 5f;
    public float maxWeatherTimer = 20f;
    private float weatherTimer = 10f;
    public float minDayNightTimer = 5f;
    public float maxDayNightTimer = 20f; 
    private float dayNightTimer = 10f;

    void Start(){
        audioSource = GetComponent<AudioSource>();
        audioSeason = transform.Find("SeasonAmbient").GetComponent<AudioSource>();
        audioWeather = transform.Find("WeatherAmbient").GetComponent<AudioSource>();
        audioDayNight = transform.Find("DayNightAmbient").GetComponent<AudioSource>();

        seasonTimer = Random.Range(minSeasonTimer, maxSeasonTimer);
        weatherTimer = Random.Range(minWeatherTimer, maxWeatherTimer);
        dayNightTimer = Random.Range(minDayNightTimer, maxDayNightTimer);
        // <Edit later> Assign the enum variable here
    }

    void Update() {
        TimeBetween -= Time.deltaTime;
        seasonTimer -= Time.deltaTime;
        weatherTimer -= Time.deltaTime;
        dayNightTimer -= Time.deltaTime;
    }
    void LateUpdate() {
        // <Delete later>
        /* if(!audioSource.isPlaying && TimeBetween <= 0){
            Play(Sunny);
            TimeBetween = Random.Range(minTimeBetween, maxTimeBetween);
        }*/

        if(!audioSeason.isPlaying && seasonTimer <= 0){
            PlaySeason();
            seasonTimer = Random.Range(minSeasonTimer, maxSeasonTimer);
        }
        if(!audioWeather.isPlaying && weatherTimer <= 0){
            PlayWeather();
            weatherTimer = Random.Range(minWeatherTimer, maxWeatherTimer);
        }
        if(!audioDayNight.isPlaying && dayNightTimer <= 0){
            PlayDayNight();
            dayNightTimer = Random.Range(minDayNightTimer, maxDayNightTimer);
        }
    }
    public void Play(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }
    public void PlayRandom(AudioClip[] clips){
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
    public void PlayRandomAll(){
        AudioClip[] seasons = {Lava, Snow, Snowing, SteamUp};
        AudioClip[] weather = {Wind, Storm, Rain, HeavyThunder, RainAndThunder};
        AudioClip[] daynight = {Ghostly, Night, Sunny, NightFrog};
        AudioClip[] other = {Frogs, Crickets, BirdsCricketsDogs, Wolves, Raven};

        int r = Random.Range(0, 4);
        AudioClip clip;
        if(r == 0){
             clip = seasons[Random.Range(0, seasons.Length)];
        } else if(r == 1){
             clip = weather[Random.Range(0, weather.Length)];
        } else if(r == 2){
             clip = daynight[Random.Range(0, daynight.Length)];
        } else if(r == 3){
             clip = other[Random.Range(0, other.Length)];
        }
        //audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
    void StopSounds(){
        audioSource.enabled = false;
        audioSource.enabled = true;
    }

    void PlaySeason(){
        AudioClip clip = null;
        // <Edit later> add more ambient
        if(Season == Seasons.Normal){

        } else if (Season == Seasons.Spring){
            clip = Sunny;
        } else if (Season == Seasons.Summer){
            clip = Lava;
        } else if (Season == Seasons.Fall){
            clip = Raven;
        } else if (Season == Seasons.Winter){
            clip = Snowing;
        }
        audioSeason.clip = clip;
        if(audioSeason.clip!=null){
            audioSeason.Play();
        }
        //Debug.Log("PlaySeason");
    }
    void PlayWeather(){
        AudioClip clip = null;
        // <Edit later> add more ambient
        if(Weather == Weathers.Normal){
            
        } else if(Weather == Weathers.Rain){
            clip = Rain;
        } else if(Weather == Weathers.Storm){
            clip = Storm; 
        } else if(Weather == Weathers.Windy){
            clip = Wind;
        }
        audioWeather.clip = clip;
        if(audioWeather!=null){
            audioWeather.Play();
        }
        //Debug.Log("PlayWeather");
    }
    void PlayDayNight(){
        AudioClip clip = null;
        // <Edit later> add more ambient
        if(DayNight == DayNightTime.Normal){
            
        } else if(DayNight == DayNightTime.Day){
            clip = MorningBird;
        } else if(DayNight == DayNightTime.Night){
            clip = Frogs;
        }
        audioDayNight.clip = clip;
        if(audioDayNight!=null){
            audioDayNight.Play();
        }
        //Debug.Log("PlayDayNight");
    }
}
