using UnityEngine;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
    private GameObject background;//背景对象
    private float bg_speed = 2.0f;//背景滚动速度,数值越小速度越快

    public int score = 0;//分数
    public GameObject gameover;
    public Text score_txt;//分数文本对象
    public Text hp_txt;
    public Text bulletnum_txt;

    public GameObject[] enemys;

    // BGM
    public AudioClip bgm_clip;

    public float bgm_volume = 0.12f;

    public AudioSource bgm_source;

    private void Start()
    {
        background = GameObject.Find("BG");//获取背景对象
        enemys = Resources.LoadAll<GameObject>("Enemys");//加载所有敌机预制体
        InvokeRepeating("Create_Enemys", 0, 1.0f);
        gameover.SetActive(false);

        // 初始化并播放 BGM（如果已设置）
        if (bgm_clip != null)
        {
            bgm_source = gameObject.AddComponent<AudioSource>();
            bgm_source.clip = bgm_clip;
            bgm_source.loop = true;
            bgm_source.playOnAwake = false;
            bgm_source.volume = bgm_volume;
            bgm_source.Play();
        }
    }

    private void Update()
    {
        background.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0f, Time.time / bg_speed));
        score_txt.text = "Score: " + score.ToString();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerControl pc = player.GetComponent<PlayerControl>();
            hp_txt.text = "HP: " + pc.hp.ToString();
            bulletnum_txt.text = pc.bullet_num.ToString();
        }
    }

    private void Create_Enemys()
    {
        int num = Random.Range(0, enemys.Length);
        GameObject enemy = Instantiate(enemys[num],
            new Vector3(Random.Range(-8f, 8f), 5.7f, 1.0f),
            Quaternion.identity
        );
        enemy.AddComponent<EnemyAI>();//给敌机添加脚本组件
        float speed = Random.Range(3.0f, 6.0f);
        enemy.GetComponent<EnemyAI>().enemy_speed = speed;//速度
        enemy.tag = "Enemy";
    }

    public void Set_Score(int val)
    {
        score += val;
    }

    public void Game_Over()
    {
        gameover.SetActive(true);
        Time.timeScale = 0f;

        bgm_source.Stop();
    }
}