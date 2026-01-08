using UnityEngine;
using UnityEngine.SceneManagement;

public class StopMenu : MonoBehaviour
{
    public GameObject stop_menu;

    private void Start()
    {
        stop_menu.SetActive(false);
    }

    private void Update()
    {
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

    public void Game_Stop()//‘“Õﬂ¬≥∂‡
    {
        Time.timeScale = 0f;
        stop_menu.SetActive(true);
        //‘›Õ£“Ù¿÷
        GameManage gm = GameObject.Find("GameManage").GetComponent<GameManage>();
        gm.bgm_source.Pause();
    }

    public void Game_Continue()//ºÃ–¯”Œœ∑
    {
        Time.timeScale = 1f;
        stop_menu.SetActive(false);
        GameManage gm = GameObject.Find("GameManage").GetComponent<GameManage>();
        gm.bgm_source.UnPause();
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