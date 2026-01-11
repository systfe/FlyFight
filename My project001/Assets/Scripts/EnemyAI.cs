using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]//请求添加碰撞盒组件
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : Plane
{
    public float enemy_speed;

    // Start is called before the first frame update
    private new void Start()
    {
        bullet_type = "NormalBullet";
        name = "Enemy";
        base.Start();
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;

        InvokeRepeating(nameof(Attack), 0, 1.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * enemy_speed);
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
            GameObject.Find("GameManage").SendMessage("Set_Score", 10, SendMessageOptions.DontRequireReceiver);
            collision.SendMessage("Damage", 90, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }

    public new bool Damage(int damage)//受到伤害
    {
        bool die = base.Damage(damage);
        if (die)
        {
            Destroy(gameObject);
        }
        else
        {
        }
        return die;
    }
}