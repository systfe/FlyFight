using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]//请求添加碰撞盒组件
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    public int hp = 100;
    public float enemy_speed;

    [SerializeField]
    private Transform[] firepos; // 支持在 Inspector 指定多个发射口

    private GameObject bullet_prefab;

    // 闪烁效果（亮度变化）相关

    // Start is called before the first frame update
    private void Start()
    {
        bullet_prefab = Resources.Load<GameObject>("NomalBullet");//加载子弹预制体
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;

        GetFire_pos();

        InvokeRepeating("Attack", 0, 1.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * enemy_speed);
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
            temp_bullet.name = "EnemyBullet";
        }
    }

    public void GetFire_pos(int pos = -1)
    {
        // 如果 Inspector 未指定发射口，则自动收集所有子Transform作为发射口
        if (firepos == null || firepos.Length == 0)
        {
            int cnt = pos == -1 ? transform.childCount : pos;
            firepos = new Transform[cnt];
            switch (pos)
            {
                case -1:
                    for (int i = 0; i < cnt; i++) firepos[i] = transform.GetChild(i);
                    break;

                case 3:
                    firepos[0] = transform.GetChild(0);
                    firepos[1] = transform.GetChild(1);
                    firepos[2] = transform.GetChild(2);
                    break;

                case 2:
                    firepos[0] = transform.GetChild(1);
                    firepos[1] = transform.GetChild(2);
                    break;

                case 1:
                    firepos[0] = transform.GetChild(0);
                    break;

                case 0:
                default:
                    break;
            }
        }

        if (firepos == null || firepos.Length == 0)
        {
            // 没有发射口就不再重复调用 Attack
            CancelInvoke(nameof(Attack));
            return;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (collision.tag == "Player")
        {
            collision.SendMessage("Damage", 90, SendMessageOptions.DontRequireReceiver);
            GameObject.Find("GameManage").SendMessage("Set_Score", 10, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }

    public bool Damage(int damage)//受到伤害
    {
        if (hp > 0) hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            Destroy(gameObject);
            return true;
        }
        else
        {
            //TODO: 发光一下
            return false;
        }
    }
}