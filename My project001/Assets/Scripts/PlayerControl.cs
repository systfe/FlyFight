using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]//请求添加碰撞盒组件
public class PlayerControl : MonoBehaviour
{
    public int hp = 100;
    public int bullet_num = 1000;
    public GameObject bullet_prefab;
    private Transform[] firepos;
    public AudioClip clips;

    // 平滑跟随参数
    private float smooth_time = 0.07f; // 平滑时间（越小越快）

    private float move_speed = 5f; // 键盘移动速度（单位：单位/秒）
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetpos;

    // Start is called before the first frame update
    private void Start()
    {
        bullet_prefab = Resources.Load<GameObject>("BoobBullet");//加载子弹预制体
        GetFire_pos(3);
        targetpos = transform.position;
        //十秒回5点血
        InvokeRepeating(nameof(FBH), 10f, 10f);
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInput();

        // 平滑移动到目标位置（无论鼠标或键盘更新 targetPos）
        transform.position = Vector3.SmoothDamp(transform.position, targetpos, ref velocity, smooth_time);

        With_Keyboard(KeyCode.Space, nameof(Attack), 0.1f);
        With_Keyboard(KeyCode.Q, nameof(RangeAttack), 1.0f);
    }

    private void HandleInput()//处理输入
    {
        // 鼠标控制优先：如果鼠标左键按下，目标位置设为鼠标位置
        // 忽略当指针位于 UI 元素上时的点击（例如点击停止按钮）
        if (Input.GetMouseButton(0) && !IsPointerOverUI())
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//获取鼠标位置

            mousePos.z = 1; // 保持 z
            mousePos.x = Mathf.Clamp(mousePos.x, -8.08f, 8.08f);//限制飞机x轴移动范围
            mousePos.y = Mathf.Clamp(mousePos.y, -3.9f, 4.4f);//限制飞机y轴移动范围
            targetpos = mousePos;
            return;
        }

        // 否则使用 WASD 键盘移动（平滑更新 targetPos）
        Vector3 inputDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) inputDir.y += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) inputDir.y -= 1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) inputDir.x -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) inputDir.x += 1f;

        if (inputDir != Vector3.zero)
        {
            inputDir.Normalize();
            targetpos += inputDir * move_speed * Time.deltaTime;

            // 限制范围
            targetpos.x = Mathf.Clamp(targetpos.x, -2.7f, 2.7f);
            targetpos.y = Mathf.Clamp(targetpos.y, -4.68f, 4.68f);
            targetpos.z = 1f; // 保持 z
        }
    }

    // 检查指针是否在 UI 元素上（兼容触摸）
    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        return EventSystem.current.IsPointerOverGameObject();
#else
                if (Input.touchCount > 0)
                    return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
                return false;
#endif
    }

    private void With_Keyboard(KeyCode thekey, string fn_name, float time_rate = 1.0f)//按键持续触发函数
    {
        if (Input.GetKeyDown(thekey))
            InvokeRepeating(fn_name, 0f, time_rate);
        if (Input.GetKeyUp(thekey))
            CancelInvoke(fn_name);
    }

    private void Attack()//普通攻击
    {
        if (bullet_num == 0 || bullet_prefab == null || firepos == null || firepos.Length == 0)
            return;

        // 对每个发射口生成一颗子弹
        foreach (var fp in firepos)
        {
            if (fp == null) continue;
            GameObject temp_bullet = Instantiate(bullet_prefab, fp.position, fp.rotation);//在发射口位置生成子弹
            temp_bullet.AddComponent<BulletControl>();//给子弹添加脚本组件
            temp_bullet.name = "PlayerBullet";//命名子弹对象

            Ammunition();//减少子弹数量
        }
        AudioSource.PlayClipAtPoint(clips, targetpos, 0.5f);//播放音效
    }

    private void RangeAttack()//特殊攻击
    {
        if (bullet_num == 0) return;
        int spread_angle = 160;
        int num = 10;
        float angle = spread_angle / (num - 1);
        for (int i = -num / 2; i < num / 2 + 1; i++)
        {
            GameObject temp_bullet = Instantiate(bullet_prefab, firepos[0].position, firepos[0].rotation);
            temp_bullet.transform.Rotate(0, 0, angle * i);
            temp_bullet.AddComponent<BulletControl>();//给子弹添加脚本组件
            temp_bullet.name = "PlayerBullet";
            Ammunition();
        }
        AudioSource.PlayClipAtPoint(clips, firepos[0].position, 1.0f);
    }

    private void Ammunition(int b = -1)
    {
        if (bullet_num > 0)
            bullet_num += b;
        if (bullet_num <= 0)
        {
            bullet_num = 0;
        }
    }

    public void GetFire_pos(int pos = -1)
    {
        // 如果 Inspector 未指定发射口，则自动收集所有子Transform作为发射口
        if (firepos == null || firepos.Length == 0)
        {
            int cnt = pos == -1 ? transform.childCount : pos;
            firepos = new Transform[cnt];
            // 指定数量的发射口
            //1->0;2->1,2;3->0,1,2;
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

    private void FBH()
    { FlashBack(5, false); }

    private void FlashBack(int v, bool effects = true)//回血
    {
        hp = hp + v >= 100 ? 100 : hp + v;
        if (effects)
        {
            // 可以添加回血反馈效果，如闪烁等
        }
    }

    private void Damage(int damage)//受到伤害
    {
        if (hp > 0) hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            Destroy(gameObject);//销毁玩家对象

            //播放爆炸特效
            //GameObject explosion_prefab = Resources.Load<GameObject>("ExplosionEffect");
            //Instantiate(explosion_prefab, transform.position, Quaternion.identity);
            GameObject.Find("GameManage").SendMessage("Game_Over", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            // 可以添加受伤反馈效果，如闪烁等
        }
    }
}