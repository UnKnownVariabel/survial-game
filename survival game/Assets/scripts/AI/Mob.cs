using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mob : MovingObject
{
    public float attackRange;

    protected int state;
    protected Path path;
    protected Vector2 direction;

    private (bool x, bool y) Done_translate;
    private Vector2 lastPos;
    private Vector2 lastDir;

    protected override void Start()
    {
        state = 1;
        base.Start();
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
                ToPlayer();
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

    protected void ToPlayer()
    {
        float distance = Vector2.Distance(transform.position, Globals.player.transform.position);
        if (path != null)
        {
            if (FollowPath())
            {
                if (distance > attackRange)
                {
                    SettPathTo(Globals.player.transform.position);
                }
                else
                {
                    path = null;
                    state = 2;
                }
            }
            else if(distance < path.Length && path.Length > 1)
            {
                SettPathTo(Globals.player.transform.position);
            }
        }
        else if (distance > attackRange)
        {
            SettPathTo(Globals.player.transform.position);
        }
        else
        {
            state = 2;
        }
    }

    protected void CloseUp()
    {
        direction = Globals.player.transform.position - transform.position;
        if (direction.sqrMagnitude > attackRange * attackRange)
        {
            state = 1;
        }
        direction.Normalize();
        setDirection(direction);
        if ((DateTime.Now - lastSwing).TotalSeconds > baseSwingTime)
        {
            attack();
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
        path = Pathfinder.createPath(transform.position, goal, 20, 1000);
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
        lastPos = transform.position;
        setDirection(direction);
        return false;

    }
}