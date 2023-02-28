using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float damage = 20;
    public LayerMask layerMask;
    public GameObject projectileInGround;
    private float startSpeed;
    private float minSpeed;

    private void Start()
    {
        startSpeed = rb.velocity.magnitude;
        minSpeed = startSpeed * 0.7f;
    }
    public void FixedUpdate()
    {
        if(rb.velocity.magnitude < minSpeed)
        {
            Instantiate(projectileInGround, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((layerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            if(collision.gameObject.TryGetComponent<DestructibleObject>(out DestructibleObject target))
            {
                target.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
