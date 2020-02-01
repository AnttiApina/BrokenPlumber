using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rgd2d;
    private Vector2 cur_velo = Vector2.zero;
    
    private List<ContactPoint2D> contact_list = new List<ContactPoint2D>();

    private bool is_grounded = false;
    private bool pressed_jump = false;
    public bool pressed_repair = false;
    [SerializeField]
    [Range(0, 40)]
    private float m_speed = 10.0f;
    
    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float m_smooth_time = .05f;
    
    [SerializeField]
    [Range(0, 40)]
    private float m_jump_velocity = 5f;
    
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        _rgd2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        pressed_jump |= Input.GetButtonDown("Jump");
        pressed_repair |= Input.GetKeyDown(KeyCode.R);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var contactsCount = _rgd2d.GetContacts(contact_list);
        is_grounded = (contact_list.Take(contactsCount).Any(contact => contact.normal.y > 0.9f));

        Vector2 velocity;
        var newVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * m_speed, (velocity = this._rgd2d.velocity).y);
        _rgd2d.velocity = Vector2.SmoothDamp(velocity, newVelocity, ref cur_velo, m_smooth_time);
        
        //Debug.Log("Is grounded " + is_grounded);
        //Debug.Log("Is pressed " + pressed_jump);
        if (is_grounded && pressed_jump)
        {
            is_grounded = false;
            _rgd2d.AddForce(new Vector2(0f, m_jump_velocity * 50));
            Debug.Log("PRESSED JUMP");
        }

        pressed_jump = false;
        
        if (pressed_repair)
        {
            StartCoroutine(PlayerIsFixing());
        }
    }

    IEnumerator PlayerIsFixing()
    {
        yield return new WaitForSeconds(0.1f);
        pressed_repair = false;
    }
}
