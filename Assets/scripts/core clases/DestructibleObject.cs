using UnityEngine;

// DestructubleObject is the class all destructible objects inherit from.
public class DestructibleObject : MonoBehaviour
{
    [System.Serializable]

    // Drop is a struct with the item to be dropt and the probability that it will be dropped.
    public struct Drop
    {
        public ItemData item;
        public float probability;
    }

    public float maxHealth;    
    public int minVisDamage;        
    public Chunk chunk;
    public int type;
    public float targetDesirebility;

    [SerializeField] protected SoundEffectHandler soundEffectHandler;
    [SerializeField] protected Item itemPrefab;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private DestructibleObject Corpse;
    [SerializeField] private Drop[] drops;
    [SerializeField] private ParticleSystem particlesystem;
    public float health
    {
        get
        {
            return _health;
        }
    }
    private float _health;

    // Awake is called when script instance is loaded.
    protected virtual void Awake()
    {
        SetHealth(maxHealth);
    }

    // Start is called before the first frame update.
    protected virtual void Start()
    {
        Globals.destructibleObjects.Add(this);
        if(targetDesirebility > 0)
        {
            Globals.targets.Add(this);
        }
    }

    // SetHealth is a setter for _health
    public void SetHealth(float health)
    {
        if(health <= maxHealth)
        {
            _health = health;
        }
        else
        {
            _health = maxHealth;
        }
    }

    // AddHealth adds health to _health
    protected void AddHealth(float value)
    {
        _health += value;
        if(_health > maxHealth)
        {
            _health = maxHealth;
        }
        if (healthBar != null)
        {
            healthBar.SetHealth(_health / maxHealth);
        }
    }

    // TakeDamage remove from _health and calls Die if _health <= 0.
    public void TakeDamage(float damage)
    {
        int temp = (int)_health / minVisDamage;
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
        else
        {
            if(healthBar != null)
            {
                healthBar.SetHealth(_health / maxHealth);
            }
            if (temp != (int)_health / minVisDamage)
            {
                if(particlesystem != null)
                {
                    particlesystem.Play();
                }
                if (soundEffectHandler != null)
                {
                    soundEffectHandler.PlayClip(0);
                }
            }
        }
    }

    // Die Destoys gameObject and is often overriden by childclasses to add behaviour.
    protected virtual void Die()
    {
        for (int i = 0; i < drops.Length; i++)
        {
            if (Random.Range(0f, 1f) <= drops[i].probability)
            {
                Item item = Instantiate(itemPrefab, transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)), Quaternion.identity);
                item.data = drops[i].item;
                Globals.chunks[(Mathf.RoundToInt(item.transform.position.x / 16), Mathf.RoundToInt(item.transform.position.y / 16))].AddItem(item);
            }
        }
        if(Corpse != null)
        {
            DestructibleObject corpse = Instantiate(Corpse, transform.position, Quaternion.identity);
            corpse.chunk = chunk;
        }
        Destroy(gameObject);
    }

    // OnDestroy is called when gameObject is destroyed.
    protected virtual void OnDestroy()
    {
        Globals.destructibleObjects.Remove(this);
        if (targetDesirebility > 0)
        {
            Globals.targets.Remove(this);
        }
    }
}
