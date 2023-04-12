using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Vector2 direction;
    public float distance;

    [SerializeField] private float loiteringTime;
    [SerializeField] private float knockback;
    [SerializeField] private float damage;
    [SerializeField] private float smallRadius;
    [SerializeField] private float largeRadius;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem particleSystem;

    private float startTime;
    private void Start()
    {
        rb.velocity = direction * distance / loiteringTime;
        startTime = Time.time;
    }
    private void Update()
    {
        if(Time.time - startTime > loiteringTime)
        {
            Explode();
        }
    }

    private void Explode()
    {
        anim.SetTrigger("explode");
        particleSystem.Play();

        Collider2D[] smallRadiusEnemys = Physics2D.OverlapCircleAll(transform.position, smallRadius);
        Collider2D[] largeRadiusEnemys = Physics2D.OverlapCircleAll(transform.position, largeRadius);

        for (int i = 0; i < smallRadiusEnemys.Length; i++)
        {
            if (smallRadiusEnemys[i].gameObject != gameObject)
            {
                if (smallRadiusEnemys[i].gameObject.TryGetComponent(out DestructibleObject Object))
                {
                    Object.TakeDamage(damage / 2);
                }
                try
                {
                    MovingObject movingObject = (MovingObject)Object;
                    movingObject.Knockback(knockback * (Object.transform.position - transform.position).normalized / 2);
                }
                catch
                {

                }
            }
        }

        for (int i = 0; i < largeRadiusEnemys.Length; i++)
        {
            if (largeRadiusEnemys[i].gameObject != gameObject)
            {
                if (largeRadiusEnemys[i].gameObject.TryGetComponent(out DestructibleObject Object))
                {
                    Object.TakeDamage(damage / 2);
                }
                try
                {
                    MovingObject movingObject = (MovingObject)Object;
                    movingObject.Knockback(knockback * (Object.transform.position - transform.position).normalized / 2);
                }
                catch
                {

                }
            }
        }

    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
