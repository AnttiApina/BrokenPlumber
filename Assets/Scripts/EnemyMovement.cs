using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Mushroomable
{
    public Sprite grimStateSprite;
    public Sprite mushroomStateSprite;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rgd2d;
    private LevelManager _levelManager;

    private float speed = 3f;
    
    
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rgd2d = GetComponent<Rigidbody2D>();
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        _rgd2d.velocity = new Vector2(speed, _rgd2d.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WALL"))
        {
            speed *= -1;
            return;
        }

        PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            speed *= -1;
            if (LevelManager.isMushroomMode)
            {
                _levelManager.InterruptMushroomMode();
            }
        }

    }

    public override void MushroomEffect(bool isMushroomState)
    {
        if (isMushroomState)
        {
            _spriteRenderer.sprite = mushroomStateSprite;
        }
        else
        {
            _spriteRenderer.sprite = grimStateSprite;
        }
        
    }
}
