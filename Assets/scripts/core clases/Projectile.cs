using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float damage = 20;
    public LayerMask layerMask;
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
