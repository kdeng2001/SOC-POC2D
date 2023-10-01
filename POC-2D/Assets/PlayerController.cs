using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private float maxSpeed;
    public float consumeTime = 0.001f;
    public float consumeCooldown = 2f;
    public Camera cam;
    private PlayerStats playerStats;
    //public GameObject dotPrefab;

    //Consume and dash
    private bool canConsume = true;
    private bool isConsuming = false;
    private float dashPower = 12f;
    public TrailRenderer tr;

    private Rigidbody2D rb;
    private Vector2 direction;
    private BoxCollider2D normalCollider;
    private SpriteRenderer normalRenderer;

    private float baseSize;
    private float baseHealth;
    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = speed;
        baseSize = gameObject. transform.localScale.x;
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        normalCollider = GetComponent<BoxCollider2D>();
        normalRenderer = GetComponent<SpriteRenderer>();

        baseHealth = playerStats.health;
    }

    // Update is called once per frame
    void Update()
    {
        if(isConsuming)
        {
            return;
        }
        float directionX = Input.GetAxisRaw("Horizontal");
        float directionY = Input.GetAxisRaw("Vertical");

        direction = new Vector2 (directionX, directionY).normalized;

        if(Input.GetButtonDown("Fire1") && canConsume)
        {
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePosition);
            Vector2 direction = (mousePosition - transform.position).normalized;
            //StartCoroutine(Consume(directionX, directionY));
            StartCoroutine(Consume(direction.x, direction.y));
        }


        if(Input.GetButtonDown("Fire2"))
        {
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePosition);
            Vector2 direction = (mousePosition - transform.position).normalized;
            //direction = cam.ScreenToWorldPoint(direction).normalized;
            Throw(direction);
        }

        if(Input.GetButtonDown("Cancel"))
        {
            MenuManager.current.ToggleMenu();
        }



    }

    private void FixedUpdate()
    {
        if (isConsuming)
        {
            return;
        }
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);

    }

    IEnumerator Consume(float directionX, float directionY)
    {
        //Debug.Log("Consume");
        normalCollider.enabled = false;
        normalRenderer.enabled = false;
        canConsume = false;
        isConsuming = true;
        rb.velocity = new Vector2(directionX, directionY).normalized * dashPower;
        tr.emitting = true;
        transform.localScale *= 1.2f;
        yield return new WaitForSeconds(consumeTime);
        transform.localScale /= 1.2f;
        tr.emitting = false;
        isConsuming = false;
        normalCollider.enabled = true;
        normalRenderer.enabled = true;
        yield return new WaitForSeconds(consumeCooldown);
        canConsume = true;      
    }

    private void Throw(Vector2 direction)
    {
        if(playerStats.health >= 15)
        {
            // throw projectile
            Debug.Log("throw");
            GameObject projectile = SlimeProjectilePool.SharedInstance.GetPooledObject();
            projectile.transform.position = transform.position;
            projectile.SetActive(true);
            projectile.GetComponent<SlimeProjectile>().direction = new Vector3(direction.x, direction.y, 0);
            UpdateHealth(-1f);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isConsuming)
        {

            float pX = normalRenderer.bounds.size.x / 2;
            float pY = normalRenderer.bounds.size.y / 2;
            float pPosX = transform.position.x;
            float pPosY = transform.position.y;
            
            Vector2 cSize = collision.GetComponent<SpriteRenderer>().bounds.size;
            float cX = cSize.x / 2;
            float cY = cSize.y / 2;
            float cPosX = collision.transform.position.x;
            float cPosY = collision.transform.position.y;

            // Successful Consume
            if( ((pPosX + pX) > (cPosX + cX)) && 
                ((pPosX - pX) < (cPosX - cX)) && 
                ((pPosY + pY) > (cPosY + cY)) && 
                ((pPosY - pY) < (cPosY - cY))
                )
            {
                // kills enemy and grows
                collision.gameObject.GetComponent<EnemyAI>().UpdateHealth(100f);
                UpdateHealth(0.2f * collision.gameObject.transform.localScale.x);
            }
            //Debug.Log("trigger stay");
        }
        
    }

    public void UpdateHealth(float healthAmount)
    {
        playerStats.UpdateHealth(healthAmount * 10f);
        UpdateSpeed();
        UpdateSize();

    }

    private void UpdateSpeed()
    {
        if (playerStats.health < baseHealth)
        {
            speed = maxSpeed;
        }
        else if(playerStats.health > baseHealth)
        {
            speed = maxSpeed - playerStats.health / 10f;
        }
    }

    private void UpdateSize()
    {
        //Vector3 newScale = new Vector3(transform.localScale.x + growthAmount, 
        //        transform.localScale.y + growthAmount, transform.localScale.z + growthAmount);
        Vector3 newScale = new Vector3(baseSize, baseSize, baseSize);
        if(playerStats.health > baseHealth)
        { 
            newScale = new Vector3(playerStats.health / 10f, playerStats.health / 10f, playerStats.health / 10f);
        }
        
        Debug.Log("Grow: " + newScale);
        transform.localScale = newScale;
        

    }
}
