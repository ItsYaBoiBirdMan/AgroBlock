using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private List<GameObject> tasksText;

    private List<Task> _currentTasks;


    private void UpdateDisplayedTasks()
    {
        for (int i = 0; i < tasksText.Count; i++)
        {
            tasksText[i].GetComponent<Text>().text = _currentTasks[i].GetTitle();
            tasksText[i].transform.GetChild(0).GetComponent<Slider>().maxValue = _currentTasks[i].GetProgressGoal();
        }
    }

    public void UpdateProgressBar()
    {
        for (int i = 0; i < tasksText.Count; i++)
        {
            tasksText[i].transform.GetChild(0).GetComponent<Slider>().value = _currentTasks[i].GetCurrentProgress();
        }
    }

    public void SimulateTaskProgress(int index)
    {
        int addedProgress = Mathf.RoundToInt(_currentTasks[index].GetProgressGoal() * (25 / 100f));
        _currentTasks[index].AddToCurrentProgressValue(addedProgress);
        UpdateDisplayedTasks();
    }
    
    private void Start()
    {
        _currentTasks = new List<Task>();
        _currentTasks = taskManager.GetCurrentTasks();
        
        UpdateDisplayedTasks();
    }
}