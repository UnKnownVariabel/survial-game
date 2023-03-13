using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mob : MovingObject
{
    public float attackRange;
    public float minRange = 0.7f;
    public Fire fire;
    public DestructibleObject target;
    public bool targetIsStatic;

    protected int state;
    protected Path path;
    protected Vector2 direction;

    private Vector2 lastDir;

    protected override void Start()
    {
        Globals.mobs.Add(this);
        state = 1;
        base.Start();
    }

    protected override void die()
    {
        Globals.mobs.Remove(this);
        base.die();
    }

    protected override void Update()
    {
        direction = Vector2.zero;
        switch (state)
        {
            case 0:
                Idle();
                break;
            case 1:
                ToTarget();
                break;
            case 2:
                CloseUp();
                break;
            default:
                Debug.LogError("state is out of bounds");
                break;
        }
        Move(direction);
        base.Update();
    }

    protected void Idle()
    {

    }

    protected void ToTarget()
    {
        if(target != null)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (path != null)
            {
                if (FollowPath())
                {
                    if (distance > attackRange)
                    {
                        SettPathTo(target.transform.position);
                    }
                    else
                    {
                        path = null;
                        state = 2;
                    }
                }
                else if (distance < path.Length * 1.5 && path.Length > 1)
                {
                    Debug.Log("this happens often");
                    SettPathTo(target.transform.position);
                }
                else if (distance < attackRange)
                {
                    state = 2;
                }
            }
            else if (distance > attackRange)
            {
                FindTarget();
            }
            else
            {
                state = 2;
            }
        }
        else
        {
            FindTarget();
        }
    }
    protected void CloseUp()
    {
        if (target == null)
        {
            if (!path.complete)
            {
                target = path.nextTarget();
                if (target != null)
                {
                    state = 1;
                    return;
                }
            }
        }
        direction = target.transform.position - transform.position;
        float distance = direction.magnitude;
        direction.Normalize();
        setDirection(direction);
        if (distance > attackRange)
        {
            state = 1;
        }
        else if (distance < minRange)
        {
            direction = Vector2.zero;
        }
        if ((DateTime.Now - lastSwing).TotalSeconds > baseSwingTime)
        {
            attack(layerMask);
        } 
    }



    protected bool FollowPath()
    {
        if (moveToStaticPoint(path.current))
        {
            lastDir = Vector2.zero;
            return !path.getNext();
        }
        return false;
    }

    protected void SettPathTo(Vector2 goal)
    {
        path = Pathfinder.createPath(transform.position, goal, 20, 1000, target, baseDamage / baseSwingTime);
        lastDir = Vector2.zero;
    }

    protected bool moveToStaticPoint(Vector2 position)
    {
        direction = (position - (Vector2)transform.position).normalized;
        if ((lastDir - direction).sqrMagnitude > 1.1)
        {
            direction = lastDir;
            return true;
        }
        lastDir = direction;
        setDirection(direction);
        return false;

    }
    public void Morning()
    {
        Fire newFire = Instantiate(fire, transform.position + new Vector3(0, 0, -1), Quaternion.identity, transform);
        newFire.target = this;
    }
    // finds target and path to target
    public void FindTarget()
    {
        float bestScore = 0;
        DestructibleObject bestTarget = null;
        foreach(DestructibleObject target in Globals.targets)
        {
            float score = target.targetDesirebility / Vector2.Distance(target.transform.position, transform.position);
            if(score > bestScore)
            {
                bestScore = score;
                bestTarget = target;
            }
        }
        target = bestTarget;
        SettPathTo(target.transform.position);
        if(path.intermedietTarget != null)
        {
            target = path.intermedietTarget;
        }
        try
        {
            StaticObject temp = (StaticObject)target;
            targetIsStatic = true;
        }
        catch
        {
            targetIsStatic = false;
        }
    }
}