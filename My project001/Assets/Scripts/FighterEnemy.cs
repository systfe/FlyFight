using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterEnemy : EnemyPlane
{
    // Start is called before the first frame update
    protected new void Start()
    {
        enemy_score = 20;
        enemy_speed = Random.Range(2.5f, 5.0f);
        hp_max = 50;
        base.Start();
    }

    // Update is called once per frame
}