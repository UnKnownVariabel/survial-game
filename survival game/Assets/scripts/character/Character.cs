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
    public Sprite[] sprites = new Sprite[4];

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
        float DPS = TileManager.instance.getDPS(transform.position);
        if (DPS != 0)
        {
            TakeDamage(Time.deltaTime * DPS);
        }
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
            SpriteRenderer.sprite = sprites[0];
        }
        else if (direction.y > 0.7)
        {
            SpriteRenderer.sprite = sprites[2];
        }
        else if (direction.x < -0.7)
        {
            SpriteRenderer.sprite = sprites[1];
        }
        else
        {
            SpriteRenderer.sprite = sprites[3];
        }
    }
    /*public void PointTo(Vector3 target)
    {
        setDirection(((Vector2)(target - transform.position)).normalized);
        Vector2 direction = ((Vector2)target- (Vector2)transform.position).normalized;
        WeaponPos.up = direction;
    }*/
    /*public void Attack()
    {
        Weapon.attack();
    }*/
    /*protected override void die()
    {
        
        if (GetComponent<soldierBotSCR>() != null)
        {
            GetComponent<soldierBotSCR>().die();
        }
        characterManagmentSCR.characters.Remove(this);
        base.die();
    }*/
    /*public void createWeapon()
    {
        Weapon = Instantiate(WeaponPref, WeaponPos.position, WeaponPos.rotation).GetComponent<weapon>();
        Weapon.transform.SetParent(WeaponPos);
        Weapon.character = this;
        if(gameObject.GetComponent<mainCharacterSCR>() != null)
        {
            gameObject.GetComponent<mainCharacterSCR>().renderers.Add(Weapon.Renderer);
        }
    }*/
}
