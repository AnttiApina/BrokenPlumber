using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaBehaviour : Mushroomable
{
    public GameObject grimState;
    public GameObject mushroomState;
    
    public override void MushroomEffect(bool isMushroomState)
    {
        grimState.SetActive(!isMushroomState);
        mushroomState.SetActive(isMushroomState);
    }
}
