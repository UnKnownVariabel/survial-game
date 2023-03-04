using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public float maxHealth;
    public float health
    {
        get
        {
            return realHealth;
        }
    }
    private float realHealth;
    public int minVisDamage;
    private ParticleSystem particlesystem;
    public HealthBar healthBar;
    public bool isDead;
    public Item itemPrefab;
    public ItemData[] itemDrops;
    public DestructibleObject Corpse;
    public Chunk chunk;
    //public byte staticIndex;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        setHealth();
    }

    protected virtual void Start()
    {
        Globals.destructibleObjects.Add(this);
        particlesystem = GetComponent<ParticleSystem>();
        if(particlesystem == null)
        {
            //Debug.Log("particle system not found");
        }
    }
    public void setHealth()
    {
        realHealth = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        int temp = (int)realHealth / minVisDamage;
        realHealth -= damage;
        if (realHealth <= 0)
        {
            die();
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
            }
        }
    }
    protected virtual void die()
    {
        for(int i = 0; i < itemDrops.Length; i++)
        {
            Item itemInstance = Instantiate(itemPrefab, transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)), Quaternion.identity);
            itemInstance.data = itemDrops[i];
        }
        if(Corpse != null)
        {
            DestructibleObject corpse = Instantiate(Corpse, transform.position, Quaternion.identity);
            corpse.chunk = chunk;
        }
        Globals.destructibleObjects.Remove(this);
        Destroy(gameObject);
    }
}
