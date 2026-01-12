using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPlane : Plane
{
    public float enemy_speed;
    public int enemy_score;

    protected new void Start()
    {
        bullet_type = nameof(NormalBullet);
        tag = "Enemy";
        base.Start();
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;

        InvokeRepeating(nameof(Attack), 0, 1.0f);
    }

    protected void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * enemy_speed);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (collision.tag == "Player")
        {
            GameObject.Find("GameManage").SendMessage("Add_Score", enemy_score, SendMessageOptions.DontRequireReceiver);
            collision.SendMessage("Damage", 90, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }

    public new bool Damage(float damage)// ‹µΩ…À∫¶
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