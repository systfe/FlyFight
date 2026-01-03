using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    private float plane_speed = 5.0f;
    private int plane_life = 100;
    public int enemy_life = 100;
    private Transform[] firepos;
    private GameObject bullet_prefab;

    private void Start()
    {
        bullet_prefab = Resources.Load<GameObject>("Bullet");//加载子弹预制体
        GetFire_pos();
        InvokeRepeating("Attack", 0, 0.1f);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void Attack()
    {
        if (bullet_prefab == null || firepos == null || firepos.Length == 0)
            return;

        // 对每个发射口生成一颗子弹
        foreach (var fp in firepos)
        {
            if (fp == null) continue;
            GameObject temp_bullet = Instantiate(bullet_prefab, fp.position, fp.rotation);

            temp_bullet.AddComponent<BulletControl>();//给子弹添加脚本组件
        }
    }

    private void GetFire_pos()
    {
        // 如果 Inspector 未指定发射口，则自动收集所有子Transform作为发射口
        if (firepos == null || firepos.Length == 0)
        {
            int cnt = transform.childCount;
            if (cnt > 0)
            {
                firepos = new Transform[cnt];
                for (int i = 0; i < cnt; i++)
                {
                    firepos[i] = transform.GetChild(i);
                }
            }
        }

        if (firepos == null || firepos.Length == 0)
        {
            // 没有发射口就不再重复调用 Attack
            CancelInvoke(nameof(Attack));
            return;
        }

        if (bullet_prefab == null)
        {
            Debug.LogError("EnemyAI: 未找到子弹预制体，请将子弹放到 Resources/Bullet.prefab 或在脚本中使用 Inspector 指定。");
            CancelInvoke(nameof(Attack));
            return;
        }
    }
}