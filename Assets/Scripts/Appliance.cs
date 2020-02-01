using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Appliance : Mushroomable
{
    private SpriteRenderer _spriteRenderer;
    public Sprite workingState;
    public Sprite brokenState;

    public bool isWorking = true;
    public int order = 0;
    
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = isWorking ? workingState : brokenState;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null && playerMovement.pressed_repair && !isWorking)
        {
            isWorking = true;
            _spriteRenderer.sprite = workingState;
            LevelManager.ApplianceRepairedEvent += RepairAppliance;
        }
    }
    
    Appliance RepairAppliance()
    {
        return this;
    }

    public void SetBroken()
    {
        isWorking = false;
        _spriteRenderer.sprite = brokenState;
    }

    public override void MushroomEffect(bool isMushroomState)
    {
        if (isMushroomState)
        {
            Debug.Log("IT'S MUSHROOMTIME!!!");
        }
        else
        {
            Debug.Log("No mushrooms anymore");
        }
    }
}
