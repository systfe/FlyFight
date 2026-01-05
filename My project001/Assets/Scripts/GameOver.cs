using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject gameover;
    public Text title;
    public Text description;
    public Button begins_again;
    public Button return_start;

    public GameOver(GameManage gm)
    {
        this.Show(false);
    }

    public void Show(bool show)
    {
        gameover.SetActive(show);
    }

    public void Game_End(string des)
    {
        description.text = des;
        Show(true);

        // Hide the pause button (if StopMenu exists in the scene)
        StopMenu stopMenu = FindObjectOfType<StopMenu>();
        stopMenu.pauseButton.gameObject.SetActive(false);
    }
}