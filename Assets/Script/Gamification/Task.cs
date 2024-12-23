using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Task Objects/Task")]
public class Task : ScriptableObject
{
    [SerializeField] public string taskTitle;
    [SerializeField] private string taskDesc;
    [SerializeField] public string taskDifficulty;
    [SerializeField] private int progressGoalValue;
    [SerializeField] private int progressCurrentValue;
    [SerializeField] private int pointsReward;
    

    public string GetTitle()
    {
        return taskTitle;
    }
    public void SetTitle(string taskname) {
        taskTitle = taskname;
    }

    public string GetTaskDifficulty() {
        return taskDifficulty;
    }
    public void SetTaskDifficulty(string difficulty) {
        taskDifficulty = difficulty;
    }

    public string GetDesc()
    {
        return taskDesc;
    }

    public int GetPointsReward()
    {
        return pointsReward;
    }   
    
    public int GetProgressGoal()
    {
        return progressGoalValue;
    }

    public int GetCurrentProgress()
    {
        return progressCurrentValue;
    }

    public void AddToCurrentProgressValue(int value)
    {
        if (progressCurrentValue + value <= progressGoalValue) progressCurrentValue += value;
        else progressCurrentValue = progressGoalValue;
    }

    public void RestProgress()
    {
        progressCurrentValue = 0;
    }
    public bool CheckIfTaskIsComplete()
    {
        return progressCurrentValue == progressGoalValue;
    }
    
    
}
