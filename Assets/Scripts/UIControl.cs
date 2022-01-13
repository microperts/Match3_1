using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public static UIControl Instance;

    public GameObject losePanel;
    
    private void Awake()
    {
        Instance = this;
    }
}
