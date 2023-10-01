using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ProjectileManager current;
    private void Awake()
    {
        if (current != this && current != null)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
        }
    }

    public Action<EnemyAI, float> onSlimeProjectileImpact;

    public void SlimeProjectileImpact(EnemyAI enemy, float effectTime)
    {
        if(onSlimeProjectileImpact != null)
        {

            onSlimeProjectileImpact(enemy, effectTime);
        }
    }
}
