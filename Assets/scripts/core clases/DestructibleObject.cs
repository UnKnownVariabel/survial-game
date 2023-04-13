using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [System.Serializable]
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
            return realHealth;
        }
    }
    private float realHealth;
    protected virtual void Awake()
    {
        SetHealth(maxHealth);
    }

    protected virtual void Start()
    {
        Globals.destructibleObjects.Add(this);
        if(targetDesirebility > 0)
        {
            Globals.targets.Add(this);
        }
    }
    public void SetHealth(float health)
    {
        if(health <= maxHealth)
        {
            realHealth = health;
        }
        else
        {
            realHealth = maxHealth;
        }
    }
    protected void AddHealth(float value)
    {
        realHealth += value;
        if(realHealth > maxHealth)
        {
            realHealth = maxHealth;
        }
        if (healthBar != null)
        {
            healthBar.SetHealth(realHealth / maxHealth);
        }
    }
    public void TakeDamage(float damage)
    {
        int temp = (int)realHealth / minVisDamage;
        realHealth -= damage;
        if (realHealth <= 0)
        {
            Die();
        }
        else
        {
            if(healthBar != null)
            {
                healthBar.SetHealth(realHealth / maxHealth);
            }
            if (temp != (int)realHealth / minVisDamage)
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
    protected virtual void OnDestroy()
    {
        Globals.destructibleObjects.Remove(this);
        if (targetDesirebility > 0)
        {
            Globals.targets.Remove(this);
        }
    }
}
