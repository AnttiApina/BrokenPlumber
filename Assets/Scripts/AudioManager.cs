using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audio;
    private Coroutine _coroutine;
    public AudioMixer _mixer;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        LevelManager.MushroomEffectChangeEvent += OnMushroomChange;
        _mixer.SetFloat("TripSfxVolume", Mathf.Log10(0.0001f) * 20);
        _mixer.SetFloat("TripMusic", Mathf.Log10(0.0001f) * 20);
    }
    
    void OnMushroomChange(bool isMushroomState)
    {
        if (isMushroomState)
        {
            onEnableMushrooms();
        }
        else
        {
            onDisableMushrooms();
        }
    }

    void onEnableMushrooms()
    {
        _audio.Play();
        StartCoroutine(FadeMixer(_mixer, "TripMusic", 4f, 1f));

        StartCoroutine(FadeMixer(_mixer, "TripSfxVolume", 4f, 1f));
        StartCoroutine(FadeMixer(_mixer, "NormalVolume", 2f, 0f));

    }
    
    void onDisableMushrooms()
    {
        StopAllCoroutines();
        StartCoroutine(FadeMixer(_mixer, "TripMusic", 8f, 0f));
        StartCoroutine(FadeMixer(_mixer, "TripSfxVolume", 8f, 0f));
        StartCoroutine(FadeMixer(_mixer, "NormalVolume", 15f, 1f));

        
    }
    

    public static IEnumerator FadeMixer(AudioMixer mixer, string param, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        mixer.GetFloat(param, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            mixer.SetFloat(param, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
    
    public void OnDisable()
    {
        LevelManager.MushroomEffectChangeEvent -= OnMushroomChange;
    }
}
