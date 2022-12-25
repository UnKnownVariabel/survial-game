using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : DestructibleObject
{
    public float movementSpeed = 4;
    //public weapon Weapon;
    //public Transform WeaponPos;
    //public GameObject WeaponPref;
    public Rigidbody2D rb;
    public SpriteRenderer SpriteRenderer;
    //public SpriteRenderer FetherRenderer;
    public Sprite[] sprites = new Sprite[4 * 3];
    public float frameTime = 0.5f;
    private float animState;
    private int dir;

    protected override void Awake()
    {
        base.Awake();
        //createWeapon();
    }

    protected override void Start()
    {
        base.Start();
        Globals.characters.Add(this);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        /*float DPS = TileManager.instance.getDPS(transform.position);
        if (DPS != 0)
        {
            TakeDamage(Time.deltaTime * DPS);
        }*/
        anim();
    }
    public void Move(Vector2 movement)
    {
        if(movement.magnitude > 1)
        {
            movement.Normalize();
        }
        //Debug.Log(TileManager.instance.DPS[1, 1]);  
        rb.velocity = movement * movementSpeed * Globals.currentChunk.getSpeed(transform.position);
    }
    protected void setDirection(Vector2 direction)
    {
        
        if (direction.y < -0.7)
        {
            //SpriteRenderer.sprite = sprites[0];
            dir = 0;
        }
        else if (direction.y > 0.7)
        {
            //SpriteRenderer.sprite = sprites[2];
            dir = 2;
        }
        else if (direction.x < -0.7)
        {
            //SpriteRenderer.sprite = sprites[1];
            dir = 1;
        }
        else
        {
            //SpriteRenderer.sprite = sprites[3];
            dir = 3;
        }
    }
    protected void anim()
    {
        if(rb.velocity == Vector2.zero)
        {
            SpriteRenderer.sprite = sprites[dir * 3];
        }
        else
        {
            animState += Time.deltaTime;
            animState = animState % (frameTime * 3);
            SpriteRenderer.sprite = sprites[dir * 3 + Mathf.FloorToInt(animState / frameTime)];
        }
    }
}
