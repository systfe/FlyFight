using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]//请求添加碰撞盒组件
public class Plane : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material default_material;
    public float hp_max;
    public float hp;
    public string bullet_type;
    protected GameObject bullet_prefab;

    protected Transform[] firepos; // 支持在 Inspector 指定多个发射口
    public int fp_num = -1; // 发射口数量，-1表示自动收集所有子Transform作为发射口

    // Start is called before the first frame update
    protected void Start()
    {
        hp = hp_max;
        spriteRenderer = GetComponent<SpriteRenderer>(); // 获取SpriteRenderer组件
        default_material = spriteRenderer.material;
        bullet_prefab = Resources.Load<GameObject>(bullet_type);//加载子弹预制体
        GetFire_pos();
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected void Attack()
    {
        if (bullet_prefab == null || firepos == null || firepos.Length == 0) return;

        // 对每个发射口生成一颗子弹
        foreach (var fp in firepos)
        {
            if (fp != null) Fire(fp);
        }
    }

    protected GameObject Fire(Transform fp)
    {
        GameObject temp_bullet = Instantiate(bullet_prefab, fp.position, fp.rotation);
        temp_bullet.name = tag + '-' + bullet_type;
        return temp_bullet;
    }

    public void GetFire_pos()
    {
        // 如果 Inspector 未指定发射口，则自动收集所有子Transform作为发射口
        if (firepos == null || firepos.Length == 0)
        {
            int cnt = fp_num == -1 ? transform.childCount : fp_num;
            firepos = new Transform[cnt];
            switch (fp_num)
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
                    CancelInvoke(nameof(Attack));
                    break;
            }
        }
    }

    private IEnumerator FlashEffect()
    {
        float flashDuration = 0.08f;

        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = Resources.Load<Material>("Material/FlashFXMaterial");
        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.material = default_material;
    }

    public bool Damage(float damage)//受到伤害
    {
        if (hp > 0) hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            return true;
        }
        else
        {
            //TODO: 发光一下
            StartCoroutine(FlashEffect());
            return false;
        }
    }
}