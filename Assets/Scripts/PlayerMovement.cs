using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rgd2d;
    private Vector2 cur_velo = Vector2.zero;
    
    [SerializeField]
    [Range(0, 40)]
    private float m_speed = 10.0f;
    
    [SerializeField]
    [Range(0, 40)]
    private float m_smooth_time = .05f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        _rgd2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 velocity;
        var newVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * m_speed, (velocity = this._rgd2d.velocity).y);
        _rgd2d.velocity = Vector2.SmoothDamp(velocity, newVelocity, ref cur_velo, 0.2f);
    }
}
