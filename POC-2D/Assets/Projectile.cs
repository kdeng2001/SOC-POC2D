using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectileAttribute/ProjectileSettings")]
public class Projectile : ScriptableObject
{
    public GameObject objToInstantiate;
    public float damage;
    public float speed;
    public float explosionTimer;
    public float effectRadiusIncrease;
    public float effectTime;
    public float despawnTimer;
}
