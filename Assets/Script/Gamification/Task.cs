using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Task Objects/Task")]
public class Task : ScriptableObject
{
    [SerializeField] private string taskTitle;
    [SerializeField] private string taskDesc;
    [SerializeField] private int progressGoalValue;
    [SerializeField] private int progressCurrentValue;
    [SerializeField] private int pointsReward;

    public string GetTitle()
    {
        return taskTitle;
    }

    public string GetDesc()
    {
        return taskDesc;
    }

    public int GetPointsReward()
    {
        return pointsReward;
    }   
    
    public bool CheckIfTaskIsComplete()
    {
        if (progressCurrentValue == progressGoalValue) return true;
        else return false;
    }
}
