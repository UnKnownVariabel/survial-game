using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : MonoBehaviour
{
    public Vector2 direction;
    public float distance;
    public float loiteringTime;

    [SerializeField] private float knockback;
    [SerializeField] private float damage;
    [SerializeField] private float smallRadius;
    [SerializeField] private float largeRadius;
    [SerializeField] private float baseScale;
    [SerializeField] private float maxExtraScale;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem particleSystem;

    private float startTime;
    private bool isExploding = false;
    private void Start()
    {
        rb.velocity = direction * distance / loiteringTime;
        startTime = Time.time;
        transform.localScale = new Vector3(baseScale, baseScale, baseScale);
    }
    private void Update()
    {
        float time = Time.time - startTime;
        if(time > loiteringTime)
        {
            if (!isExploding)
            {
                isExploding = true;
                Explode();
            }
        }
        else
        {
            float scale = -(2 * time / loiteringTime - 1) * (2 * time / loiteringTime - 1) * maxExtraScale + baseScale + maxExtraScale;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void Explode()
    {
        rb.velocity = new Vector2(0, 0);
        anim.SetTrigger("explode");
        particleSystem.Play();

        Collider2D[] smallRadiusEnemys = Physics2D.OverlapCircleAll(transform.position, smallRadius, layerMask);
        Collider2D[] largeRadiusEnemys = Physics2D.OverlapCircleAll(transform.position, largeRadius, layerMask);

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
