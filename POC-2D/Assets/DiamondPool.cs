using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPool : ObjectPool
{
    // Start is called before the first frame update
    public static DiamondPool SharedInstance;
    public override void Awake()
    {
        SharedInstance = this;
    }
}
