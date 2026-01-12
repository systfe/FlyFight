using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyEnemy : EnemyPlane
{
    private new void Start()
    {
        enemy_speed = Random.Range(5.0f, 7.5f);
        hp_max = 50;
        base.Start();
        enemy_score = 10;
    }

    public new void GetFire_pos()
    {
        return;
    }

    private new void Attack()
    {
        return;
    }
}