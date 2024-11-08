using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private List<GameObject> tasksText;

    [SerializeField]private List<Task> _currentTasks;


    private void UpdateDisplayedTasks()
    {
        for (int i = 0; i < tasksText.Count; i++)
        {
            if(!_currentTasks[i]) continue;
            tasksText[i].GetComponent<Text>().text = _currentTasks[i].GetTitle();
            tasksText[i].GetComponent<Text>().color = Color.white;
            tasksText[i].transform.GetChild(0).GetComponent<Slider>().maxValue = _currentTasks[i].GetProgressGoal();
            tasksText[i].transform.GetChild(0).GetComponent<Slider>().value = _currentTasks[i].GetCurrentProgress();
            tasksText[i].transform.GetChild(1).GetComponent<Text>().text = _currentTasks[i].GetDesc();
        }
    }

    private void UpdateProgressBar()
    {
        for (int i = 0; i < tasksText.Count; i++)
        {
            if(!_currentTasks[i]) continue;
            tasksText[i].transform.GetChild(0).GetComponent<Slider>().value = _currentTasks[i].GetCurrentProgress();
        }
    }

    private void RemoveDisplayedTaskWhenCompleted(int index)
    {
        tasksText[index].GetComponent<Text>().text = "Task Completed";
        tasksText[index].GetComponent<Text>().color = Color.gray;
    }
    public void SimulateTaskProgress(int index)
    {
        if (_currentTasks[index] != null)
        {
            var addedProgress = Mathf.RoundToInt(_currentTasks[index].GetProgressGoal() * (25 / 100f));
            if (addedProgress < 1) addedProgress = 1;
            _currentTasks[index].AddToCurrentProgressValue(addedProgress);
            UpdateDisplayedTasks();
            if (_currentTasks[index].GetCurrentProgress() == _currentTasks[index].GetProgressGoal()) RemoveDisplayedTaskWhenCompleted(index);
            UpdateProgressBar();
        }
        else Debug.Log("That Task is already completed");
    }

    public void GetNewTasks()
    {
        _currentTasks = new List<Task>();
        taskManager.RefreshTasks();
        _currentTasks = taskManager.GetCurrentTasks();
        UpdateDisplayedTasks();
        Debug.Log("Refresh");
    }
    private void Start()
    {
        _currentTasks = new List<Task>();
        _currentTasks = taskManager.GetCurrentTasks();
        
        UpdateDisplayedTasks();
    }
}