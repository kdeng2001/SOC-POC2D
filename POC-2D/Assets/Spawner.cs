using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectPrefab;
    public float spawnCooldown;
    //public float xSpawnMin;
    //public float xSpawnMax;
    //public float ySpawnMin;
    //public float ySpawnMax;

    private float spawnTime;
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - spawnTime > spawnCooldown)
        {
            SpawnObject();
            spawnTime = Time.time;
        }
    }

    private void SpawnObject()
    {
        GameObject enemy = DiamondPool.SharedInstance.GetPooledObject();
        if(enemy != null)
        {
            //Vector3 newPosition = new Vector3(Random.Range(xSpawnMin, xSpawnMax), Random.Range(ySpawnMin, ySpawnMax), 0);
            //enemy.transform.position = newPosition;
            enemy.transform.position = transform.position;
            //float scaleVal = Random.Range(1, 10);
            //enemy.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
            enemy.SetActive(true);
        }

        
    }
}
