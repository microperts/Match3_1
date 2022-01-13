using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour 
{
    public Text textElement;
    public Text textElement_extra;

    public static int Value
    {
        get => PlayerPrefs.GetInt("score", 0);
        set => PlayerPrefs.SetInt("score", value);
    }

    public static void Increment(int value)
    {
        Value = Value + value;
    }
    
    private void Update()
    {
        textElement.text = Value.ToString();
        textElement_extra.text = Value.ToString();
    }
}
