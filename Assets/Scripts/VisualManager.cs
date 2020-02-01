using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    private Camera camera;
     
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
        }
    
    void onDisableMushrooms()
    {
        
    }
}
