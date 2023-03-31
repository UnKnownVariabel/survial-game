using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovingObject : DestructibleObject
{
    public float maxSpeed = 4;
    public float acceleration = 1;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public float frameTime = 0.5f;
    public Transform pivotTransform;
    public BoxCollider2D damageCollider;
    public LayerMask layerMask;
    public float baseDamage = 2;
    public float baseSwingTime = 1;
    public float baseKnockback = 10;

    protected float lastSwing;
    protected int dir;
    protected float animState;
    

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
        Anim();
    }
    public void Move(Vector2 movement)
    {
        if(movement.magnitude > 1)
        {
            movement.Normalize();
        }
        //Debug.Log(TileManager.instance.DPS[1, 1]);
        Vector2 goal_velocity = movement * maxSpeed * Globals.GetChunk(transform.position).GetSpeed(transform.position);
        Vector2 acceleration_direction = (goal_velocity - rb.velocity).normalized;
        rb.velocity = rb.velocity + acceleration_direction * acceleration * Time.deltaTime;
        if((acceleration_direction.x < 0 && goal_velocity.x - rb.velocity.x > 0) || (acceleration_direction.x > 0 && goal_velocity.x - rb.velocity.x < 0) || (acceleration_direction.y < 0 && goal_velocity.y - rb.velocity.y > 0) || (acceleration_direction.y > 0 && goal_velocity.y - rb.velocity.y < 0))
        {
            rb.velocity = goal_velocity;
        }
        if(Globals.GetChunk(transform.position).GetDPS(transform.position) > 0)
        {
            TakeDamage(Globals.GetChunk(transform.position).GetDPS(transform.position) * Time.deltaTime);
        }
    }
    protected virtual void SetDirection(Vector2 direction)
    {
        if(direction == Vector2.zero)
        {
            return;
        }
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
    protected virtual void Anim()
    {
        if(rb.velocity == Vector2.zero)
        {
            spriteRenderer.sprite = sprites[dir * 3];
        }
        else
        {
            animState += Time.deltaTime;
            animState = animState % (frameTime * 3);
            spriteRenderer.sprite = sprites[dir * 3 + Mathf.FloorToInt(animState / frameTime)];
        }
    }
    protected virtual int? Attack(float damage, Vector2 extraOffset, LayerMask layerMask, float knockback, Multipliers multipliers)
    {
        Vector2 Direction = pivotTransform.right;
        Vector2 boxPos = Rotate(new Vector2(damageCollider.offset.x, damageCollider.offset.y * pivotTransform.localScale.y) + extraOffset, Direction) + (Vector2)pivotTransform.transform.position;
        lastSwing = Time.time;
        Collider2D[] enemys = Physics2D.OverlapBoxAll(boxPos, damageCollider.size, pivotTransform.eulerAngles.z, layerMask);
        int? type = null;

        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].gameObject != gameObject)
            {
                if (enemys[i].gameObject.TryGetComponent(out DestructibleObject Object))
                {
                    type = Object.type;
                    switch (Object.type)
                    {
                        case 0:
                            Object.TakeDamage(damage * multipliers.mob);
                            break;
                        case 1:
                            Object.TakeDamage(damage * multipliers.wood); 
                            break;
                        case 2:
                            Object.TakeDamage(damage * multipliers.stone);
                            break;
                    }
                    
                    try
                    {
                        MovingObject movingObject = (MovingObject)Object;
                        movingObject.Knockback(knockback * (Object.transform.position - transform.position).normalized);
                    }
                    catch
                    {

                    }
                }
            }
        }
        return type;
        Vector2 Rotate(Vector2 vector, Vector2 rotation)
        {
            return new Vector2(rotation.x * vector.x - rotation.y * vector.y, rotation.y * vector.x + rotation.x * vector.y);
        }
    }
    protected int? Attack(LayerMask layerMask)
    {
        return Attack(baseDamage, new Vector2(0, 0), layerMask, baseKnockback, Multipliers.One);
    }
    public void Knockback(Vector2 dir)
    {
        rb.velocity += dir;
    }
}
