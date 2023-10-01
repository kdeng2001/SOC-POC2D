using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

public class SlimeProjectilePool : ObjectPool
{
    public static SlimeProjectilePool SharedInstance;
    public override void Awake()
    {
        SharedInstance = this;
    }
}
