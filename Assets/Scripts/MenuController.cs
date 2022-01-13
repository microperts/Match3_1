using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static bool returningPlay = false;

    public GameObject loadingPanel;

    private void OnEnable()
    {
        Time.timeScale = 1.0f;
        if (returningPlay)
        {
            OnClick_Play();
        }
        else
        {
            loadingPanel.SetActive(false);
        }
    }

    public void OnClick_Play()
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
    }
}
