using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    public Text textElement;
    
    public static int Value
    {
        get => PlayerPrefs.GetInt("turn", 0);
        set => PlayerPrefs.SetInt("turn", value);
    }

    public static void Increment(int value)
    {
        Value = Value + value;
    }

    private void Update()
    {
        textElement.text = Value.ToString();
    }
}
