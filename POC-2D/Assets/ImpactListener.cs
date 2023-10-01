using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ProjectileManager.current.onSlimeProjectileImpact += SlowEnemy;
    }

    public void SlowEnemy(EnemyAI enemy, float effectTime)
    {
        StartCoroutine(Slow(enemy, effectTime));
    }

    IEnumerator Slow(EnemyAI enemy, float effectTime)
    {
        if (enemy != null)
        {
            // lower speed of enemy
            float enemyInitSpeed = enemy.speed;
            enemy.speed /= 5;
            yield return new WaitForSeconds(effectTime);
            // revert speed of enemy back to normal
            enemy.speed = enemyInitSpeed;
        }
        // destroy projectile
        
        
        

    }

}
