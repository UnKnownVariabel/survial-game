using System.Collections;
using UnityEngine;

// Tower is the class attached to all towers and it inherits from the class Building.
// Towers are autonomous weapons which attack mobs.
public class Tower : Building
{
    [SerializeField] protected Mob target;
    [SerializeField] protected Transform spawnPoint;
    [SerializeField] protected float range;

    [SerializeField] private float chargeTime;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private bool isLoaded;
    [SerializeField] private Animator animator;
    [SerializeField] private float timeBetweenUpdates;

    private IEnumerator coroutine;

    // Start is called before the first frame update.
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

    // Update is called once per frame.
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

    // UpdateTarget calls AquireTarget and waits specified time then repeats.
    IEnumerator UpdateTarget()
    {
        target = AquireTarget();
        coroutine = UpdateTarget();
        yield return new WaitForSeconds(timeBetweenUpdates);
        StartCoroutine(coroutine);
    }

    // AquireTarget searches all mobs for the closest one.
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

    // Shoot starts shooting animation.
    private void Shoot()
    {
        animator.SetTrigger("shoot");
        isLoaded = false;
    }

    // SpawnProjectile spawnProjectile and gives it its velocity
    public virtual void SpawnProjectile()
    {
        Projectile proj = Instantiate(projectile, spawnPoint.position, pivotTransform.rotation);
        proj.rb.velocity = pivotTransform.up * projectileSpeed;
    }

    // LoadingDone is called when loading animation is done.
    public void LoadingDone()
    {
        isLoaded = true;
    }
}
