using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    public static Turn Instance;
    
    public Text textElement;

    public static UnityEvent onZero;
    
    public static int Value
    {
        get => PlayerPrefs.GetInt("turn", 0);
        set => PlayerPrefs.SetInt("turn", value);
    }

    private void Awake()
    {
        Instance = this;
    }

    public static void Increment(int value)
    {
        Value = Value + value;
    }
    
    public static void Decrement(int value)
    {
        Value = Value - value;
        if (Value <= 0)
        {
            Instance._DelayedShow();
        }
    }

    public void _DelayedShow()
    {
        this.Invoke(() =>
        {
            if (Value > 0) { return; }
            Time.timeScale = 0.00000001f;
            UIControl.Instance.losePanel.SetActive(true);
            onZero?.Invoke();
        },1.0f);
    }

    private void Update()
    {
        ///- Turn Debug
        if (Input.GetKeyDown(KeyCode.T))
        {
            Value += 10;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            Score.Value += 10;
        }
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Value = 1;
        }
        
        textElement.text = Value.ToString();
    }
}
