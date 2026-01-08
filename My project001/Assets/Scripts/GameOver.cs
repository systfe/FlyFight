using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject gameover;
    public Text title;
    public Text description;
    public Button begins_again;
    public Button return_start;

    private void Update()
    {
    }

    public void Show(bool show)
    {
        gameover.SetActive(show);
    }

    public void Game_End(string des)
    {
        description.text = des;
        Show(true);
    }
}