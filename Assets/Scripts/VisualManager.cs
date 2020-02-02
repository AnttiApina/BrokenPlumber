using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    private Camera camera;
    public Animation boomClip;
    public Material pixelMaterial;
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
        
    }
    private float progress = 0f;
    void Update()
    {
    }

    IEnumerator lerpTo(float targetProgress)
    {
        float duration = 4.0f;
        float t = 0;

        var curProgress = progress;

        while (t < duration)
        {
            t += Time.deltaTime;

            var speed_t = t / duration;
            progress = Mathf.SmoothStep(curProgress, targetProgress, speed_t);

            yield return null;
        }

        yield return null;
    }
}