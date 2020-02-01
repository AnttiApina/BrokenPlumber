
using System;
using UnityEngine;

public abstract class Mushroomable : MonoBehaviour
{
    public virtual void Awake()
    {
        LevelManager.MushroomEffectChangeEvent += MushroomEffect;
    }

    public abstract void MushroomEffect(bool isMushroomState);
}
