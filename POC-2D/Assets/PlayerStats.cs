using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : Stats
{
    public GameObject gameOver;
    public GameObject retry;
    public Volume damaged;
    public Volume healed;
    public float volumeTime = 0.5f;
    private Image healthBar;

    public override void Awake()
    {
        healthBar = GameObject.Find("PlayerHealthUI").transform.GetChild(0).GetComponent<Image>();
    }

    public override void UpdateHealth(float healthAmount)
    {
        UpdateHealthUI(healthAmount);
        
        if (health <= 0)
        {
            Debug.Log("Player died");
            gameOver.SetActive(true);
            retry.SetActive(true);
            gameObject.SetActive(false);
        }

    }

    private void UpdateHealthUI(float healthChange)
    {
        Debug.Log("HealthChange: " + healthChange);
        if(healthChange > 0) { healed.weight = 1; }
        else if(healthChange < 0) { damaged.weight = 1; }
        Invoke(nameof(VolumeChange), volumeTime);
        health += healthChange;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        Debug.Log("Player Health: " + health);
        float healthPercentage = health / maxHealth;
        //Debug.Log("bruh " + healthPercentage);
        healthBar.fillAmount = healthPercentage;
        ScoreManager.current.UpdateHealth(health);

    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void VolumeChange()
    {
        healed.weight = 0;
        damaged.weight = 0;

    }
}
