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

    [SerializeField] private float timeBetweenUpdates;

    private IEnumerator coroutine;

    protected override void Start()
    {
        base.Start();
        if(timeBetweenUpdates <= 0)
        {
            Debug.LogError("time between target updates should not be 0. force set to 0.5");
            timeBetweenUpdates = 0.5f;
        }
        coroutine = UpdateTarget();
        StartCoroutine(coroutine);
    }

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
    }

    IEnumerator UpdateTarget()
    {
        target = AquireTarget();
        coroutine = UpdateTarget();
        yield return new WaitForSeconds(timeBetweenUpdates);
        StartCoroutine(coroutine);
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
