using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float damage;
    public float speed;
    private float maxSpeed;
    public LayerMask playerMask;
    
    

    //Patrolling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    public bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public Rigidbody2D rb;

    public abstract void AttackPlayer();
    public abstract void ChasePlayer();
    public abstract void Patrolling();
    private void Awake()
    {
        maxSpeed = speed;
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        speed = maxSpeed;
    }

    public void FixedUpdate()
    {
        // check for sight and attack range
        playerInSightRange = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), sightRange, playerMask);
        playerInAttackRange = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), attackRange, playerMask);
        if (!playerInSightRange && !playerInAttackRange)
        {
            //Debug.Log("Patrol");
            Patrolling();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            //Debug.Log("Chase");
            ChasePlayer();
        }


        if (playerInSightRange && playerInAttackRange)
        {
            //Debug.Log("Attack");
            AttackPlayer();
        }

    }

    public virtual void ResetAttack()
    {
        //Debug.Log("reset attack");
        alreadyAttacked = false;
    }
    public void DealDamage()
    {
        Debug.Log("deal damage");
        //player.gameObject.GetComponent<playerStats>().TakeDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    public abstract void UpdateHealth(float healthAmount);

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    rb.angularVelocity = 0f;
    //}


}
