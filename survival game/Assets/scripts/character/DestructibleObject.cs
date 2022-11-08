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
    public int team;
    public int minVisDamage;
    private ParticleSystem particlesystem;
    public HealthBar healthBar;
    public bool isDead;
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
            Debug.Log("particle system not found");
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
                particlesystem.Play();
            }
        }
    }
    protected virtual void die()
    {
        Globals.destructibleObjects.Remove(this);
        Destroy(gameObject);
    }
}
