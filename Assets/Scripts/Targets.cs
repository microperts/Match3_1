using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Targets : MonoBehaviour
{
    public static Targets Instance;

    public static Dictionary<int,int> TargetValues; 
    public static Dictionary<int,Transform> TargetTransforms; 

    public JewelSpawn jewelSpawn;
    public GameObject itemPrefab;
    public Transform content;

    private List<Text> targetTexts;

    private void Awake()
    {
        Instance = this;
        TargetValues = new Dictionary<int, int>();
        TargetTransforms = new Dictionary<int, Transform>();
    }

    public void PopulateTargets(List<int> targetList)
    {
        targetTexts = new List<Text>();
        foreach (int i in targetList)
        {
            var item = Instantiate(itemPrefab, content);
            item.GetComponentInChildren<Image>().sprite = jewelSpawn.JewelSprites[i];
            int targetValue = Random.Range(23, 36);
            TargetValues.Add(i,targetValue);
            item.GetComponentInChildren<Text>().text = targetValue.ToString();
            targetTexts.Add(item.GetComponentInChildren<Text>());
            TargetTransforms.Add(i,item.transform);
        }
    }

    public static bool IsTargetType(int targetType)
    {
        return TargetValues.ContainsKey(targetType);
    }
    
    public Transform GetTargetTransform(int targetType)
    {
        return TargetTransforms.ContainsKey(targetType) ? TargetTransforms[targetType] : GameObject.FindObjectOfType<Targets>().transform;
    }
    
    public static void TargetMatch(int targetType, int count)
    {
        if (!TargetValues.ContainsKey(targetType)) { return; }
        
        TargetValues[targetType] -= count;

        if (TargetValues[targetType] <= 0)
        {
            TargetTransforms[targetType].GetChild(2).gameObject.SetActive(true);
            TargetValues[targetType] = 0;
        }
    }
    
    private void Update()
    {
        if (targetTexts == null || targetTexts.Count <= 0) { return; }

        int index = 0;
        foreach (KeyValuePair<int,int> targetValue in TargetValues)
        {
            if (targetValue.Value < 0)
            {
                targetTexts[index].transform.parent.GetChild(2).gameObject.SetActive(true);
            }
            
            targetTexts[index].text = targetValue.Value.ToString();
            
            index++;
        }
        
        /*
        for (var i = 0; i < targetTexts.Count; i++)
        {
            Text targetText = targetTexts[i];
            targetText.text = TargetValues[i].ToString();
        }*/
    }
}
