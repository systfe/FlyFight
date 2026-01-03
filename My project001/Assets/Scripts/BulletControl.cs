using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]//请求添加碰撞盒组件
[RequireComponent(typeof(Rigidbody2D))]
public class BulletControl : MonoBehaviour
{
    private float bullet_speed = 8.0f;//子弹速度

    private void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * bullet_speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                if (this.name == "EnemyBullet")
                {
                    other.SendMessage("Damage", 20, SendMessageOptions.DontRequireReceiver);

                    Destroy(gameObject);
                }
                break;

            case "Enemy":
                if (this.name == "PlayerBullet")
                {
                    // 如果被击中的对象是敌机，并且被这次伤害击毁，则加分
                    var enemy = other.GetComponent<EnemyAI>();//尝试获取敌机脚本组件
                    if (enemy != null)
                    {
                        bool dead = enemy.Damage(20);
                        if (dead) GameObject.Find("GameManage").SendMessage("Set_Score", 10, SendMessageOptions.DontRequireReceiver);
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
}