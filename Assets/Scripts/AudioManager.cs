using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audio;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        LevelManager.MushroomEffectChangeEvent += OnMushroomChange;
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
    }
    
    void onDisableMushrooms()
    {
        
    }
}
