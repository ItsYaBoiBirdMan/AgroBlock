using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI minThreshold;
    [SerializeField] private TextMeshProUGUI maxThreshold;

    private float _min;
    private float _max;

    public void SetPercentage(float percent)
    {
        fill.fillAmount = percent / 100f;
    }

    public void SetMin(float min)
    {
        minThreshold.text = "Min: " + min;
    }
    
    public void SetMax(float max)
    {
        maxThreshold.text = "Max: " + max;
    }

    private void CheckBarStatus()
    {
        if (fill.fillAmount  >= 1)
        {
            fill.color = Color.red;
        }
        else if (fill.fillAmount <= 0)
        {
            background.color = Color.blue;
        }
        else
        {
            fill.color = new Color(0.3f, 0.9f, 0.47f);
            background.color = new Color(.7f, .7f, .7f);
        }
        
    }


    private void Start()
    {
        SetPercentage(10);
        SetMin(20);
        SetMax(90);
        
        CheckBarStatus();
    }

    private void Update()
    {
        CheckBarStatus();
    }
}
