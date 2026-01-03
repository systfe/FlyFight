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

        pauseButton.GetComponentInChildren<Text>().text = "||";
    }

    private void Update()
    {
        //ESCº¸‘›Õ£/ºÃ–¯”Œœ∑
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
        pauseButton.GetComponentInChildren<Text>().text = "|>";
    }

    public void Game_Continue()//ºÃ–¯”Œœ∑
    {
        Time.timeScale = 1f;
        stop_menu.SetActive(false);
        GameManage gm = GameObject.Find("GameManage").GetComponent<GameManage>();
        gm.bgm_source.UnPause();
        pauseButton.GetComponentInChildren<Text>().text = "||";
    }

    public void Return_Start()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
}