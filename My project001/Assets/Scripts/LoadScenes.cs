using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScenes : MonoBehaviour
{
    private static string static_target_scene;
    private AsyncOperation async;
    public Slider loading_bar;
    public Text loading_txt;

    // Start is called before the first frame update
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Loading")
        {
            async = SceneManager.LoadSceneAsync(static_target_scene);
            async.allowSceneActivation = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // 增加 async 非空保护，避免在非 Loading 场景或未初始化时访问 async 引发空引用
        if (loading_txt && loading_bar && async != null)
        {
            loading_txt.text = "Loading: " + (async.progress / 0.9f * 100).ToString("F0") + "%";
            loading_bar.value = async.progress / 0.9f;
            if (async.progress >= 0.9f)
            {
                loading_txt.text = "按下任意键以开始游戏...";
                loading_bar.value = 1f;
                // 使用 anyKeyDown 更适合检测“按下”事件，同时为兼容性再显式检测鼠标按下
                if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                {
                    async.allowSceneActivation = true;
                }
            }
        }
    }

    public void Load_Scene(string target_scene)
    {
        static_target_scene = target_scene;
        SceneManager.LoadScene("Loading");
    }

    public void Reset_Scene()
    {
        static_target_scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Loading");
    }
}