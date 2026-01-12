using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]//请求添加碰撞盒组件
[RequireComponent(typeof(Rigidbody2D))]
public class BulletControl : MonoBehaviour
{
    public float bullet_speed;//子弹速度
    public float damage;//子弹伤害

    protected void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    protected void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * bullet_speed);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                if (this.name.Contains("Enemy-"))
                {
                    other.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
                    Hit_In();
                    Destroy(gameObject);
                }
                break;

            case "Enemy":
                if (this.name.Contains("Player-"))
                {
                    // 如果被击中的对象是敌机，并且被这次伤害击毁，则加分
                    var enemy = other.GetComponent<EnemyPlane>();//尝试获取敌机脚本组件
                    if (enemy != null)
                    {
                        bool dead = enemy.Damage(damage);
                        Hit_In();
                        if (dead) GameObject.Find("GameManage").SendMessage("Add_Score", enemy.enemy_score, SendMessageOptions.DontRequireReceiver);
                    }

                    Destroy(gameObject);
                }
                break;

            case "Wall":
                Destroy(gameObject);
                break;

            default:
                if (this.name != other.name) Destroy(gameObject);
                break;
        }
    }

    void Hit_In()
    {
    }
}