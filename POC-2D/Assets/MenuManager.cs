using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager current;
    public GameObject menu;
    //private bool active = false;
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

    public void ToggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
        if (!menu.activeSelf) { Time.timeScale = 1; Debug.Log("unpause: " + menu.activeSelf); }
        else { Time.timeScale = 0; Debug.Log("pause: " + menu.activeSelf); }
    }

    public void LoadScene1()
    {
        Time.timeScale = 1;
        Debug.Log(Time.timeScale);
        SceneManager.LoadScene(0);
        
    }

    public void LoadScene2()
    {   
        Time.timeScale = 1;
        Debug.Log(Time.timeScale);
        SceneManager.LoadScene(1);
        
    }
}
