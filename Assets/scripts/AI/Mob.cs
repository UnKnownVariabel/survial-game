using UnityEngine;

// Mobs are all enemys in the game.
public class Mob : MovingObject
{
    public float attackRange;
    public float minRange = 0.7f;
    public Fire fire;
    public DestructibleObject target;
    public bool targetIsStatic;

    // State is the current mode of the mob.
    protected int state;
    protected Path path;
    protected Vector2 direction;

    private Vector2 lastDir;

    // Start is called before the first frame update.
    protected override void Start()
    {
        Globals.mobs.Add(this);
        state = 1;
        base.Start();
    }

    // Die is called when the destructibleObject runs out of health.
    protected override void Die()
    {
        Globals.mobs.Remove(this);
        base.Die();
    }

    // Update is called once per frame.
    protected override void Update()
    {
        direction = Vector2.zero;

        // Switch uses the int state to decide what function to call.
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

    // Idle is called when mob is idle
    protected void Idle()
    {

    }

    // ToTarget moves the mob towards the object defined by the variable target.
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
                else if (distance * 1.5 < path.length && path.length > 1)
                {
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

    // Is called when mob is close the target. Attacks the target and moves closer if it is to far away.
    protected void CloseUp()
    {
        if (target == null)
        {
            if (!path.complete)
            {
                target = path.NextTarget();
                if (target == null)
                {
                    state = 1;
                    return;
                }
            }
            else
            {
                state = 1;
                return;
            }
        }
        direction = target.transform.position - transform.position;
        float distance = direction.magnitude;
        direction.Normalize();
        SetDirection(direction);
        if (distance > attackRange)
        {
            state = 1;
        }
        else if (distance < minRange)
        {
            direction = Vector2.zero;
        }
        if (Time.time - lastSwing > baseSwingTime)
        {
            Attack(layerMask);
        } 
    }


    // Folows the path retuns true if the path is done.
    protected bool FollowPath()
    {
        if (moveToStaticPoint(path.current))
        {
            lastDir = Vector2.zero;
            return !path.GetNext();
        }
        return false;
    }

    // Creates path to goal.
    protected void SettPathTo(Vector2 goal)
    {
        path = Pathfinder.CreatePath(transform.position, goal, 20, 1000, target, baseDamage / baseSwingTime);
        lastDir = Vector2.zero;
    }

    // Makes mob move to a static point in world space.
    protected bool moveToStaticPoint(Vector2 position)
    {
        direction = (position - (Vector2)transform.position).normalized;
        if ((lastDir - direction).sqrMagnitude > 1.1)
        {
            direction = lastDir;
            return true;
        }
        lastDir = direction;
        SetDirection(direction);
        return false;

    }

    // Morning is called in every mob in the morning and ataches fire to the mob.
    public void Morning()
    {
        Fire newFire = Instantiate(fire, transform.position + new Vector3(0, 0, -1), Quaternion.identity, transform);
        newFire.target = this;
    }

    // Finds a target and creates a path to it.
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