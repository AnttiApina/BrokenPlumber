using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VisualManager : MonoBehaviour
{
    private Camera camera;
    public Animation boomClip;
    public Material pixelMaterial;
    public Volume volumeStack;
    void Start()
    {
        camera = Camera.main;
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
        boomClip.Play();
    }
    
    void onDisableMushrooms()
    {
        StartCoroutine(FadeMushroomEffect(volumeStack, 3f));
    }
    private float progress = 0f;
    void Update()
    {
    }

    public static IEnumerator FadeMushroomEffect(Volume vol, float duration)
    {
        float currentTime = 0;

        float startWeight = vol.weight;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            vol.weight = Mathf.Lerp(startWeight, 0f, currentTime / duration);
            yield return null;
        }
        yield break;
    }
    
    public void OnDisable()
    {
        LevelManager.MushroomEffectChangeEvent -= OnMushroomChange;
    }
}