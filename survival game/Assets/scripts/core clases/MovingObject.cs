using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovingObject : DestructibleObject
{
    public float movementSpeed = 4;
    public Rigidbody2D rb;
    public SpriteRenderer SpriteRenderer;
    public Sprite[] sprites = new Sprite[4 * 3];
    public float frameTime = 0.5f;
    public Transform pivotTransform;
    public BoxCollider2D damageCollider;
    public float baseDamage = 2;
    public float baseSwingTime = 1;

    protected DateTime lastSwing;


    private float animState;
    private int dir;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        Globals.characters.Add(this);
    }

    protected virtual void Update()
    {
        anim();
    }
    public void Move(Vector2 movement)
    {
        if(movement.magnitude > 1)
        {
            movement.Normalize();
        }
        //Debug.Log(TileManager.instance.DPS[1, 1]);  
        rb.velocity = movement * movementSpeed * Globals.GetChunk(transform.position).getSpeed(transform.position);
    }
    protected void setDirection(Vector2 direction)
    {
        
        if (direction.y < -0.7)
        {
            dir = 0;
        }
        else if (direction.y > 0.7)
        {
            dir = 2;
        }
        else if (direction.x < -0.7)
        {
            dir = 1;
        }
        else
        {
            dir = 3;
        }
        pivotTransform.right = direction;
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
    protected virtual void attack(float damage, Vector2 extraOffset)
    {
        Vector2 Direction = pivotTransform.right;
        Vector2 boxPos = Rotate(new Vector2(damageCollider.offset.x, damageCollider.offset.y * pivotTransform.localScale.y) + extraOffset, Direction) + (Vector2)pivotTransform.transform.position;
        lastSwing = DateTime.Now;
        Collider2D[] enemys = Physics2D.OverlapBoxAll(boxPos, damageCollider.size, pivotTransform.eulerAngles.z);

        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].gameObject != gameObject)
            {
                if (enemys[i].gameObject.TryGetComponent(out DestructibleObject Object))
                {
                    Object.TakeDamage(damage);
                }
            }
        }
        Vector2 Rotate(Vector2 vector, Vector2 rotation)
        {
            return new Vector2(rotation.x * vector.x - rotation.y * vector.y, rotation.y * vector.x + rotation.x * vector.y);
        }
    }
    protected void attack()
    {
        attack(baseDamage, new Vector2(0, 0));
    }
}
