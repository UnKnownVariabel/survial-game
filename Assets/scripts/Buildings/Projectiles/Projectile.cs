using UnityEngine;

// Class attached to arrow.
public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float damage = 20;
    public float knockback;
    public LayerMask layerMask;
    public GameObject projectileInGround;
    private float startSpeed;
    private float minSpeed;

    // Start is called before the first frame update.
    protected virtual void Start()
    {
        startSpeed = rb.velocity.magnitude;
        minSpeed = startSpeed * 0.7f;
    }

    // FixedUpdate is called 50 times per second.
    protected virtual void FixedUpdate()
    {
        if(rb.velocity.magnitude < minSpeed)
        {
            Instantiate(projectileInGround, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    // OnCollisionEnter2D is called when projectile collides with other collider and is used to detect if projectile has hit enemy.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((layerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            if (collision.gameObject.TryGetComponent<DestructibleObject>(out DestructibleObject target))
            {
                target.TakeDamage(damage);
                try
                {
                    MovingObject movingObject = (MovingObject)target;
                    movingObject.Knockback(knockback * (target.transform.position - transform.position).normalized);
                }
                catch
                {

                }
                Destroy(gameObject);
            }
        }
    }
}
