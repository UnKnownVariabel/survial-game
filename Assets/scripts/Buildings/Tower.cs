using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    public float range;
    public float chargeTime;
    public float projectileSpeed;
    public Projectile projectile;
    public Transform pivotTransform;
    public Transform spawnPoint;
    public bool isLoaded;
    public Animator animator;

    [SerializeField]protected Mob target;

    void Update()
    {
        if(target != null)
        {
            pivotTransform.up = target.transform.position - pivotTransform.position;
            if (isLoaded)
            {
                Shoot();
            }
        }
        else
        {
            target = AquireTarget();
        }
    }

    private Mob AquireTarget()
    {
        float shortestDistanceSqr = Mathf.Infinity;
        float distanceSqr;
        Mob target = null;
        foreach(Mob mob in Globals.mobs)
        {
            distanceSqr = ((Vector2)(mob.transform.position - pivotTransform.position)).sqrMagnitude;
            if(distanceSqr < shortestDistanceSqr)
            {
                shortestDistanceSqr = distanceSqr;
                target = mob;
            }
        }
        if(shortestDistanceSqr < range * range)
        {
            return target;
        }
        return null;
    }
    private void Shoot()
    {
        animator.SetTrigger("shoot");
        isLoaded = false;
    }
    public virtual void SpawnProjectile()
    {
        Projectile proj = Instantiate(projectile, spawnPoint.position, pivotTransform.rotation);
        proj.rb.velocity = pivotTransform.up * projectileSpeed;
    }
    public void LoadingDone()
    {
        isLoaded = true;
    }
}
