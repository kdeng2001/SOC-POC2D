using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondEnemy : EnemyAI
{
    GameObject[] AI;
    public float spaceBetween;

    public float lungeTime = 0.001f;
    public float lungeCooldown = 2f;
    //private bool canLunge = true;
    private bool isLunging = false;
    //private float lungePower = 16f;
    public TrailRenderer tr;

    public override void AttackPlayer()
    {
        if(!isLunging && !alreadyAttacked)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = Vector2.zero;
            //Debug.Log("Attack");
            alreadyAttacked = true;
            StartCoroutine(LungeAttack(direction.x, direction.y));
        }

    }
    
    IEnumerator LungeAttack(float directionX, float directionY)
    {
        //canLunge = false;
        isLunging = true;
        rb.velocity = new Vector2(directionX, directionY).normalized * speed * 1.5f;
        tr.emitting = true;
        //Debug.Log("Lunge start " + tr.emitting);
        yield return new WaitForSeconds(lungeTime + 0.5f);
        tr.emitting = false;
        //Debug.Log("isLunging is set to false at " + Time.time);
        isLunging = false;
        //Invoke(nameof(ResetAttack), timeBetweenAttacks);
        yield return new WaitForSeconds(lungeCooldown);
        alreadyAttacked = false;
        //Debug.Log("Lunge cooldown ended at " + Time.time);
        //yield return new WaitForSeconds(lungeCooldown);

    }

    public override void ChasePlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Deg2Rad;
        direction.Normalize();

        rb.MovePosition(speed * Time.deltaTime * direction + new Vector2(transform.position.x, transform.position.y));
        rb.MoveRotation(speed * Time.deltaTime * angle);
            //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            //transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    public override void Patrolling()
    {
        if(!walkPointSet)
        {
            walkPoint = new Vector3(transform.position.x + Random.Range(-walkPointRange, walkPointRange),
            transform.position.y + Random.Range(-walkPointRange, walkPointRange), 0);

            walkPointSet = true;
        }
        else
        {
            //transform.position = Vector2.MoveTowards(this.transform.position, walkPoint, 0.5f * speed * Time.deltaTime);
            Vector3 direction = walkPoint - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Deg2Rad;
            //transform.rotation = Quaternion.Euler(Vector3.forward * angle);

            rb.MovePosition(speed * Time.deltaTime * new Vector2(direction.x, direction.y) + new Vector2(transform.position.x, transform.position.y));
            rb.MoveRotation(Time.deltaTime * angle);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //Debug.Log(distanceToWalkPoint.magnitude);
        if (distanceToWalkPoint.magnitude < 5f)
        {
            walkPointSet = false;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        AI = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject go in AI)
        {
            if (go != gameObject)
            {
                if(Vector3.Distance(go.transform.position, transform.position) <= spaceBetween)
                {
                    Vector3 avoidDirection = transform.position - go.transform.position;
                    avoidDirection.Normalize();
                    rb.MovePosition(speed * Time.deltaTime * new Vector2(avoidDirection.x, avoidDirection.y) + 
                        new Vector2(transform.position.x, transform.position.y));
                }
             
            }
        }

    }

    public override void UpdateHealth(float healthAmount)
    {

        alreadyAttacked = false;
        tr.emitting = false;
        isLunging = false;
        ScoreManager.current.UpdateEaten();
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (isLunging && collision.gameObject.CompareTag("Player"))
        //if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("successful enemy attack");
            collision.gameObject.GetComponent<PlayerController>().UpdateHealth(-damage);
            ScoreManager.current.UpdateHitsTaken();
            isLunging = false;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("On Trigger: " + isLunging + " at " + collision.gameObject.name);
        }
    }
}
