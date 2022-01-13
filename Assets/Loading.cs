using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public static Loading Instance;
    
    public GameObject loadingPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void Show()
    {
        loadingPanel.SetActive(true);
    }

    public void Hide()
    {
        loadingPanel.SetActive(false);
    }
}
