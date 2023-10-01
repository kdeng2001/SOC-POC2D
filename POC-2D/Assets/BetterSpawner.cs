using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Lumin;

public class BetterSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectPrefab;
    public float[] xRange;
    public float[] yRange;

    public float[] spawnTimes; // same as level
    public float[] enemiesEatenLevelThreshold; // same as level

    public float[] maxSizeRange; // size ranges for different enemies (related to probability of spawning)
    public float[] spawnDistribution; // chance of spawning for each size


    private float eatsToNextLevel;
    public int level = 0;

    private float spawnTime;
    void Start()
    {
        spawnTime = Time.time;
        eatsToNextLevel = enemiesEatenLevelThreshold[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - spawnTime > spawnTimes[level])
        {
            SpawnObject();
            spawnTime = Time.time;
        }

        if(ScoreManager.current.eaten < enemiesEatenLevelThreshold[level])
        {
            // do nothing
        }else if(level+1 < enemiesEatenLevelThreshold.Length) { level++;}
    }

    private void SpawnObject()
    {
        GameObject enemy = DiamondPool.SharedInstance.GetPooledObject();
        if (enemy != null)
        {
            Vector3 newPosition = new Vector3(Random.Range(xRange[0], xRange[1]), Random.Range(yRange[0], yRange[1]), 0);
            enemy.transform.position = newPosition;
            //enemy.transform.position = transform.position;

            float chance = Random.Range(0, spawnDistribution[level]);

            int useLevel = 0;
            for(int lvl = 0; lvl < spawnDistribution.Length-1; lvl++)
            {
                if(chance > spawnDistribution[lvl] && chance < spawnDistribution[lvl+1])
                {
                    useLevel = lvl;
                    break;
                }
            }

            // find index of maxSizeRange, where chance < maxSizeRange[i+1], greater than maxSizeRange[i]
            float scaleVal = Random.Range(1, maxSizeRange[useLevel]);
            enemy.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
            enemy.gameObject.GetComponent<EnemyAI>().speed /= scaleVal;
            enemy.SetActive(true);
        }


    }
}
