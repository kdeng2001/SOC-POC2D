using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int hitsTaken = 0;
    public int eaten = 0;
    public TextMeshProUGUI hitsTakenText;
    public TextMeshProUGUI eatenText;
    public TextMeshProUGUI healthText;
    public static ScoreManager current;

    private void Start()
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
    public void UpdateHitsTaken()
    {
        hitsTaken++;
        hitsTakenText.text = hitsTaken.ToString("0000");

    }
    public void UpdateEaten()
    {
        eaten++;
        eatenText.text = eaten.ToString("0000");
    }

    public void UpdateHealth(float health) 
    {
        healthText.text = health.ToString("000");
    }
}
