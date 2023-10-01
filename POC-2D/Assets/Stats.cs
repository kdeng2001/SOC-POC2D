using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats : MonoBehaviour
{
    public float health;
    //public float size;
    public float maxHealth;
    //public float speed;

    public virtual void Awake()
    {
        health = maxHealth;
    }

    public virtual void UpdateHealth(float healthAmount)
    {
        health += healthAmount;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
