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

    private GameObject popupScore;

    public static event Action<int> OnIncrementInScore;

    public static void OnIncrementScoreCallBack(int x)
    {
        OnIncrementInScore?.Invoke(x);
    }

    public static int Value
    {
        get => PlayerPrefs.GetInt("turn", 0);
        set => PlayerPrefs.SetInt("turn", value);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnIncrementInScore += Increment;
    }
    public  void Increment(int value)
    {
        Value = Value + value;
        if (value == 1)
        {
            ShowPopupScore("+1");
        }else if(value == 2)
        {
            ShowPopupScore("+2");
        }
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
        /*if (Input.GetKeyDown(KeyCode.T))
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
        }*/
        
        textElement.text = Value.ToString();
    }

    public  void ShowPopupScore(string textToPrint)
    {
        // UpdateBar();

        //var parent = GameObject.Find("PopUpScore").transform;
        var popupObject = Resources.Load<GameObject>("PopupScores");
        popupScore = Instantiate(popupObject,GameObject.FindObjectOfType<UIControl>().transform);
        popupScore.GetComponentInChildren<Text>().text = textToPrint;
        //popupScore.SetActive(true);
        /*this.Invoke(() =>popupScore.SetActive(false),1.5f);*/

        //var poptxt = Instantiate(popupScore, parent.position, Quaternion.identity);
        // popupScore.SetActive(true);
        //DisableMove();
        //poptxt.transform.GetComponentInChildren<Text>().text = textToPrint;
        //if (color <= scoresColors.Length - 1)
        //{
        //    poptxt.transform.GetComponentInChildren<Text>().color = scoresColors[color];
        //    poptxt.transform.GetComponentInChildren<Outline>().effectColor = scoresColorsOutline[color];
        //}

        //poptxt.transform.SetParent(parent);

        //   poptxt.transform.position += Vector3.right * 1;
        //poptxt.transform.localScale = Vector3.one / 1.5f;
        //Destroy(poptxt, 0.9f);

    }
}
