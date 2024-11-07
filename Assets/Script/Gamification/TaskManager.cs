using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<Task> allTasks;
    [SerializeField] private List<Task> currentTasks;
    [SerializeField] private List<Task> completedTasks;

    [SerializeField] private UserDataManager userDataManager;
    
    
    private void GetRandomTasks(List<Task> allTasksList, List<Task> currentTaskList, int count)
    {
        count = Mathf.Min(count, allTasksList.Count);
        
        List<Task> tempList = new List<Task>(allTasksList);
        
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, tempList.Count);
            Task selectedElement = tempList[randomIndex];
            
            currentTaskList.Add(selectedElement);
            
            tempList.RemoveAt(randomIndex);
        }
    }
    private void CheckForCompletedTasks()
    {
        for (int i = 0; i < currentTasks.Count; i++)
        {
            if (currentTasks[i].CheckIfTaskIsComplete())
            {
                EventManager.TaskCompletedEvent.Invoke(currentTasks[i]);
                currentTasks[i].RestProgress();
                completedTasks.Add(currentTasks[i]);
                currentTasks.RemoveAt(i);
            }
        }
    }

    private void AddPointsFromCompletedTask(Task task)
    {
        userDataManager.AddPoints(task.GetPointsReward());
        Debug.Log(task.GetPointsReward() + " added!!"); 
    }

    private void Awake()
    {
        currentTasks = new List<Task>();
        completedTasks = new List<Task>();
        
        GetRandomTasks(allTasks, currentTasks, 3);
    }

    private void Update()
    {
        CheckForCompletedTasks();
    }

    public List<Task> GetCurrentTasks()
    {
        return currentTasks;
    }

    private void OnEnable()
    {
        EventManager.TaskCompletedEvent.AddListener(AddPointsFromCompletedTask);
    }

    private void OnDisable()
    {
        EventManager.TaskCompletedEvent.RemoveListener(AddPointsFromCompletedTask);
    }
}
