using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StopMenu : MonoBehaviour
{
    public GameObject stop_menu;

    public Button pauseButton;

    private void Start()
    {
        stop_menu.SetActive(false);

        if (pauseButton != null)
            pauseButton.GetComponentInChildren<Text>().text = "||";
    }

    private void Update()
    {
        //ESC键暂停/继续游戏
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1f)
            {
                Game_Stop();
            }
            else
            {
                Game_Continue();
            }
        }
    }

    public void Game_Stop()//砸瓦鲁多
    {
        Time.timeScale = 0f;
        stop_menu.SetActive(true);
        //暂停音乐
        GameManage gm = GameObject.Find("GameManage").GetComponent<GameManage>();
        gm.bgm_source.Pause();

        if (pauseButton != null)
        {
            pauseButton.GetComponentInChildren<Text>().text = "|>";
            // 隐藏暂停按钮界面元素
            pauseButton.gameObject.SetActive(false);
        }
    }

    public void Game_Continue()//继续游戏
    {
        Time.timeScale = 1f;
        stop_menu.SetActive(false);
        GameManage gm = GameObject.Find("GameManage").GetComponent<GameManage>();
        gm.bgm_source.UnPause();

        if (pauseButton != null)
        {
            // 恢复并更新文本
            pauseButton.gameObject.SetActive(true);
            pauseButton.GetComponentInChildren<Text>().text = "||";
        }
    }

    public void Begins_Again()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene("Play");
        Time.timeScale = 1f;
    }

    public void Return_Start()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
}