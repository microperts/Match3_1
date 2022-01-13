using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    public Text textElement;

    public static UnityEvent onZero;
    
    public static int Value
    {
        get => PlayerPrefs.GetInt("turn", 0);
        set => PlayerPrefs.SetInt("turn", value);
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
            Time.timeScale = 0.00000001f;
            UIControl.Instance.losePanel.SetActive(true);
            onZero?.Invoke();
        }
    }

    private void Update()
    {
        textElement.text = Value.ToString();
    }
}
