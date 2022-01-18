using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static bool ReturningPlay = false;
    public static bool ShowLeaderboard = false;

    public GameObject loadingPanel;
    public GameObject leaderboardPanel;
    public WebGLTransfer20 Transfer20;

    private void OnEnable()
    {
        Time.timeScale = 1.0f;
        if (ReturningPlay)
        {
            ReturningPlay = false;
            OnClick_Play();
        }
        else
        {
            loadingPanel.SetActive(false);
        }

        if (ShowLeaderboard)
        {
            ShowLeaderboard = false;
            leaderboardPanel.SetActive(true);
        }
    }

    public void OnClick_Play()
    {
        loadingPanel.SetActive(true);

        Transfer20.onFailure.AddListener(()=>
        {
            loadingPanel.SetActive(false);
            PopupMessage.Instance.Show("Failed to transfer, please try again later");
        });
        
        Transfer20.onSuccess.AddListener(()=>
        {
            Player p = new Player();
            p.HightScore = long.Parse(PlayerPrefs.GetString("ClassicHightScore", "0"));
            p.Level = 1;
            p.Name = "classic";
            p.Stars = 0;
            p.UnLocked = true;

            MapLoader.MapPlayer = p;
            MapLoader.Mode = 0;
            SceneManager.LoadScene(GlobalConsts.SCENE_PLAY);
        });
        
        Transfer20.Transfer();
    }
}
