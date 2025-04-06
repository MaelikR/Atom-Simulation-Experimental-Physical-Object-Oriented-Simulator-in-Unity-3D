using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UnderwaterAmbience : MonoBehaviour
{
    public AudioClip nightAmbience;
    public AudioClip abyssCrackles;
    public AudioClip whaleCalls;

    public float nightStartHour = 18f;
    public float nightEndHour = 6f;

    [Range(0f, 1f)] public float crackleVolume = 0.2f;
    [Range(0f, 1f)] public float whaleVolume = 0.3f;

    private AudioSource mainSource;
    private AudioSource crackleSource;
    private AudioSource whaleSource;

    private bool isNight = false;

    void Start()
    {
        mainSource = gameObject.AddComponent<AudioSource>();
        crackleSource = gameObject.AddComponent<AudioSource>();
        whaleSource = gameObject.AddComponent<AudioSource>();

        mainSource.loop = true;
        crackleSource.loop = true;
        whaleSource.loop = true;

        mainSource.clip = nightAmbience;
        crackleSource.clip = abyssCrackles;
        whaleSource.clip = whaleCalls;

        mainSource.volume = 0.6f;
        crackleSource.volume = crackleVolume;
        whaleSource.volume = whaleVolume;
    }

    void Update()
    {
        float time = FindCurrentHour();
        bool shouldBeNight = (time >= nightStartHour || time <= nightEndHour);

        if (shouldBeNight && !isNight)
        {
            ActivateNightAmbience();
        }
        else if (!shouldBeNight && isNight)
        {
            StopNightAmbience();
        }
    }

    float FindCurrentHour()
    {
        return DayNightCycle.CurrentHour;
    }

    void ActivateNightAmbience()
    {
        isNight = true;
        mainSource.Play();
        crackleSource.Play();
        whaleSource.Play();
    }

    void StopNightAmbience()
    {
        isNight = false;
        mainSource.Stop();
        crackleSource.Stop();
        whaleSource.Stop();
    }
}
