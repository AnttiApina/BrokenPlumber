using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DamageBehaviour : MonoBehaviour
{
    private LevelManager _levelManager;
    
    public void Awake()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
        if(playerMovement != null)
        {
            if (LevelManager.isMushroomMode)
            {
                _levelManager.InterruptMushroomMode();
            }
        }

    }
}
