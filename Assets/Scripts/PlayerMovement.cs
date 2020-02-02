using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : Mushroomable
{
    private LevelManager _levelManager;
    public static Vector3 nextFixable;

    public Vector3 initPos;
    private Rigidbody2D _rgd2d;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;

    private Vector2 cur_velo = Vector2.zero;
    
    private List<ContactPoint2D> contact_list = new List<ContactPoint2D>();

    private bool is_grounded = false;
    private bool is_dropping = false;
    private bool pressed_jump = false;
    private bool pressed_fall = false;
    public bool ladder_mode = false;
    public bool pressed_repair = false;

    LayerMask ladderModeMask;
    LayerMask ladderMask;
    ContactFilter2D  ladderModeFilter;
    
    [SerializeField]
    [Range(0, 40)]
    private float m_speed = 10.0f;

    [SerializeField]
    [Range(0, 40)]
    private float grimSpeed = 5.0f;

    private float currentSpeed;

    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float m_smooth_time = .05f;
    
    [SerializeField]
    [Range(0, 40)]
    private float m_jump_velocity = 5f;
    
    private float m_ladder_threshold = 0.4f;

    private String animTrigger = null;
    
    // Start is called before the first frame update
    private void Awake()
    {
        base.Awake();
        _levelManager = FindObjectOfType<LevelManager>();
        initPos = transform.position;
        currentSpeed = grimSpeed;
        _rgd2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        ladderModeMask = LayerMask.GetMask("Ground");
        ladderMask = LayerMask.GetMask("Ladders");

        ladderModeFilter = new ContactFilter2D();
        ladderModeFilter.useLayerMask = true;
        ladderModeFilter.SetLayerMask(ladderModeMask);
        
        
    }

    public void Respawn()
    {
        transform.position = initPos;
    }

    public override void MushroomEffect(bool isMushroomState)
    {
        currentSpeed = isMushroomState ? m_speed : grimSpeed;
    }

    private void Update()
    {
        pressed_jump |= Input.GetButtonDown("Jump") && _levelManager.isMushroomMode;
        pressed_repair |= Input.GetKeyDown(KeyCode.R);
        pressed_fall |=  Input.GetButton("Jump") && Input.GetKey(KeyCode.S) && _levelManager.isMushroomMode;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _animator.speed = _rgd2d.velocity.magnitude < 0.05f ? 0 : 1;
        if (ladder_mode)
        {
            ladderMode();
            return;            
        }

        var contactsCount = _rgd2d.GetContacts(contact_list);
        is_grounded = (contact_list.Take(contactsCount).Any(contact => contact.normal.y > 0.9f));

        if (is_grounded && pressed_fall)
        {
            _animator.SetTrigger("Jump");
            _rgd2d.velocity = Vector2.zero;
            Debug.Log("FREEFALLING");
            _boxCollider2D.isTrigger = true;
            is_dropping = true;
        } else if (is_grounded)
        {
            _animator.SetTrigger("Contact");
        }

        Vector2 velocity;
        var newSpeed = is_dropping ? 0 : currentSpeed;
        var newVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * newSpeed, (velocity = this._rgd2d.velocity).y);
        _rgd2d.velocity = Vector2.SmoothDamp(velocity, newVelocity, ref cur_velo, m_smooth_time);

        var absX = Math.Abs(velocity.x);
        if (absX > .1f)
        {
            _spriteRenderer.flipX = velocity.x > 0;
        }

        if (!ladder_mode)
        {
            _animator.SetBool("Moving", !is_dropping && absX > 0.1f);
        }

        if (is_dropping)
        {
            return;
        }
        
        if (is_grounded && pressed_jump)
        {
            _animator.SetTrigger("Jump");
            is_grounded = false;
            _rgd2d.AddForce(new Vector2(0f, m_jump_velocity * 50));
            Debug.Log("PRESSED JUMP");
        }

        
        if (pressed_repair)
        {
            StartCoroutine(PlayerIsFixing());
        }
        
        pressed_jump = false;
        pressed_fall = false;
        animTrigger = null;

        // ShowArrow();
    }

    private void ShowArrow()
    {
        Vector3 direction = nextFixable - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PASSABLE_GROUND")
        {
            _boxCollider2D.isTrigger = false;
            is_dropping = false;
            Debug.Log("stop falling");
        }
    }

    private void ladderMode()
    {
        _rgd2d.bodyType = RigidbodyType2D.Kinematic;
        var newVelocity = new Vector2(0, Input.GetAxisRaw("Vertical") * 5);
        _rgd2d.velocity = newVelocity;

        var bounds = _boxCollider2D.bounds;
        var bottomBound = bounds.extents;
        var boxCenter = bounds.center - bottomBound;
        boxCenter.x += 0.5f;
        
        Debug.DrawLine(boxCenter, boxCenter + (Vector3.down * m_ladder_threshold), Color.magenta);
        
        var hit = Physics2D.Raycast(boxCenter, Vector2.down, m_ladder_threshold, ladderModeMask);
        var contactsCount = _rgd2d.GetContacts(ladderModeFilter, contact_list);
        Debug.Log("Contacts " + contactsCount);
        
        if(contactsCount <= 0 && hit)
        { 
            _rgd2d.bodyType = RigidbodyType2D.Dynamic; 
            _animator.SetBool("Climbing", false);
            ladder_mode = false;
        }
    }

    void CheckLadderMode(Collider2D other)
    {
        if (ladder_mode || !other.CompareTag("LADDER"))
        {
            return;
        }
        
        var rigidTransformPosition = _rgd2d.transform.position;
        var ladderAbove = Physics2D.OverlapBox(rigidTransformPosition + Vector3.up * 1.25f, Vector2.one * 0.1f, 0f, ladderMask);
        var ladderBelow = Physics2D.OverlapBox(rigidTransformPosition + Vector3.down * 1.25f, Vector2.one * 0.1f, 0f, ladderMask);

        if (ladderAbove && Input.GetAxisRaw("Vertical") > 0)
        {
            rigidTransformPosition = new Vector3(other.transform.position.x, rigidTransformPosition.y + 0.25f, 0);
            _rgd2d.transform.position = rigidTransformPosition;
            ladder_mode = true;
            _animator.SetBool("Climbing", true);
        } else if (ladderBelow && Input.GetAxisRaw("Vertical") < 0)
        {
            rigidTransformPosition = new Vector3(other.transform.position.x, rigidTransformPosition.y - 0.5f, 0);
            _rgd2d.transform.position = rigidTransformPosition;
            ladder_mode = true;
            _animator.SetBool("Climbing", true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckLadderMode(other);
    }

    IEnumerator PlayerIsFixing()
    {
        yield return new WaitForSeconds(0.1f);
        pressed_repair = false;
    }
}
