using System.Collections;
using UnityEngine;

// Class atached to the bombs which come out of the mortor.
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
    [SerializeField] private float explosionTime;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private SoundEffectHandler soundEffectHandler;

    private float startTime;
    private bool isExploding = false;

    // Start is called before the first frame update.
    private void Start()
    {
        rb.velocity = direction * distance / loiteringTime;
        startTime = Time.time;
        transform.localScale = new Vector3(baseScale, baseScale, baseScale);
    }

    // Update is called once per frame.
    private void Update()
    {
        float time = Time.time - startTime;
        if(time > loiteringTime)
        {
            if (!isExploding)
            {
                isExploding = true;
                IEnumerator corutine = Explode();
                StartCoroutine(corutine);
            }
        }
        else
        {
            float scale = -(2 * time / loiteringTime - 1) * (2 * time / loiteringTime - 1) * maxExtraScale + baseScale + maxExtraScale;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    // Is called when the bomb is suposed to explode.
    private IEnumerator Explode()
    {
        rb.velocity = new Vector2(0, 0);
        sprite.enabled = false;
        particleSystem.Play();
        soundEffectHandler.PlayClip(0);

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
                if (smallRadiusEnemys[i].gameObject.TryGetComponent(out MovingObject movingObject))
                {
                    movingObject.Knockback(knockback * (Object.transform.position - transform.position).normalized / 2);
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

        yield return new WaitForSeconds(explosionTime);

        Delete();

    }

    

    // Deletes the bomb.
    public void Delete()
    {
        Destroy(gameObject);
    }
}
