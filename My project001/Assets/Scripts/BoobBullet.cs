using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoobBullet : BulletControl
{
    new void Start()
    {
        damage = 50f;
        bullet_speed = 8.5f;
        base.Start();
    }

    void Hit_In()
    {
        //播放命中特效
    }
}