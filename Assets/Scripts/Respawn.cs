using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private PlayerMovement _player;
    private LevelManager _levelManager;
    
    // Start is called before the first frame update
    void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>();
        _levelManager = FindObjectOfType<LevelManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(EnterVoid());
    }

    IEnumerator EnterVoid()
    {
        UIManager.OnUpdateEnterVoidText += () => 1;
        yield return new WaitForSeconds(3);
        _levelManager.InterruptMushroomMode();
        UIManager.OnUpdateEnterVoidText += () => 0;
        _player.Respawn();
    }
}
