using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskScreenManager : MonoBehaviour
{
    [SerializeField] private Transform TaskParent;
    [SerializeField] private List<GameObject> CurrentTasks;
    [SerializeField] private GameObject TaskPrefab;


    private void CreateNewTask(string taskName, string taskDesc, string timeLeft, string diff, int goal, int reward)
    {
        var newTask = Instantiate(TaskPrefab, TaskParent);
        newTask.GetComponent<TaskController>().SetTaskInfo(taskName, taskDesc, timeLeft, diff, goal, reward);
        CurrentTasks.Add(newTask);
    }

    private void Start()
    {
        CurrentTasks = new List<GameObject>();
    }

    public void TestTaskCreation()
    {
        CreateNewTask("Test", "Test Desc", "00:00", "Easy", 10, 1000);
    }

    private void RemoveTaskOnCompletion(GameObject task)
    {
        CurrentTasks.Remove(task);
        Destroy(task);
    }

    private void OnEnable()
    {
        EventManager.RemoveCompletedTaskEvent.AddListener(RemoveTaskOnCompletion);
    }

    private void OnDisable()
    {
        EventManager.RemoveCompletedTaskEvent.RemoveListener(RemoveTaskOnCompletion);
    }
}
