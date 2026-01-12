using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BulletControl
{
    new void Start()
    {
        damage = 20f;
        bullet_speed = 8.5f;
        base.Start();
    }
}