using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    public Projectile p;
    private float aliveTime;
    private bool projHit = false;
    private bool hasExploded = false;
    private Vector3 effectRadiusChange;
    public Vector3 direction;

    private void Awake()
    {
        effectRadiusChange = new Vector3(p.effectRadiusIncrease, p.effectRadiusIncrease, 0);
        gameObject.transform.localScale += effectRadiusChange;
    }
    private void OnEnable()
    {
        aliveTime = Time.time;
    }

    private void OnDisable()
    {
        gameObject.transform.localScale -= effectRadiusChange;
        projHit = false;
        hasExploded=false;
    }

    public void Update()
    {
        // Projectile is in mid launch
        if(!projHit && ((Time.time - aliveTime) < p.explosionTimer))
        {
            //Debug.Log("moving forward" + ((Time.time - aliveTime) + " vs. " + p.rangeTimer) + " is " + ((Time.time - aliveTime) < p.rangeTimer));
            transform.position += p.speed * Time.deltaTime * direction;
        } // Projectile hits / explodes
        else if(!hasExploded)
        {
            CreateExplosion(null);
        }

        if(Time.time - aliveTime > p.despawnTimer)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Slime Projectile trigger");
        //Debug.Log(collision.gameObject.GetComponent<EnemyAI>().speed);
        if (collision.CompareTag("Enemy")) { 
            CreateExplosion(collision.gameObject.GetComponent<EnemyAI>());
            projHit = true;
        }
    }

    private void CreateExplosion(EnemyAI enemy)
    {
        if(!hasExploded) {
            gameObject.transform.localScale += effectRadiusChange;
            hasExploded = true;
        }
        ProjectileManager.current.SlimeProjectileImpact(enemy, p.effectTime);
    }
}
