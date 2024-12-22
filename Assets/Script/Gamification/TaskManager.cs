using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<Task> allTasks;
    [SerializeField] private List<Task> currentTasks;
    [SerializeField] private List<Task> completedTasks;

    [SerializeField] private UserDataManager userDataManager;
    
    [SerializeField] private Transform completedTasksParent;
    [SerializeField] private GameObject completedTaskPrefab;
    
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
            if (currentTasks[i] == null) continue;
            if (currentTasks[i].CheckIfTaskIsComplete())
            {
                Debug.LogError("Task completed");
                EventManager.TaskCompletedEvent.Invoke(currentTasks[i]);
                currentTasks[i].RestProgress();
                completedTasks.Add(currentTasks[i]);
                SaveCompletedTasksIntoFile(completedTasks);
                DisplayCompletedTasks(completedTasks);
                currentTasks[i] = null;
            }
        }
    }

    private void AddPointsFromCompletedTask(Task task)
    {
        userDataManager.AddPoints(task.GetPointsReward());
        Debug.Log(task.GetPointsReward() + " added!!"); 
    }

    public void RefreshTasks()
    {
        for (int i = 0; i < allTasks.Count; i++)
        {
            allTasks[i].RestProgress();
        }

        currentTasks = new List<Task>();
        GetRandomTasks(allTasks, currentTasks, 3);
    }

    private void Awake()
    {
        string inputFilePath = "CompletedTasks.json"; // Ensure the correct JSON filename
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "JSON", inputFilePath);
        if (!File.Exists(jsonFilePath)){
            completedTasks = new List<Task>();
        } else {
            try {
                completedTasks = JsonConvert.DeserializeObject<List<Task>>(File.ReadAllText(jsonFilePath));
                DisplayCompletedTasks(completedTasks);
            } catch (Exception ex) {
                Debug.LogError($"Failed to load crops: {ex.Message}");
            } 
        }
        currentTasks = new List<Task>();
        
        for (int i = 0; i < allTasks.Count; i++)
        {
            allTasks[i].RestProgress();
        }
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

    public void DisplayCompletedTasks(List<Task> tasks){
        foreach (Task task in tasks){
            var newTask = Instantiate(completedTaskPrefab, completedTasksParent);
            newTask.GetComponent<CompletedTaskController>().SetTaskInfo(task.GetTitle(), task.GetTaskDifficulty());
        }
    }
    
    public void SaveCompletedTasksIntoFile(List<Task> tasks){
        // The value inside is irrelevant
        string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
        string folderPath = Path.Combine(Application.persistentDataPath, "JSON");
        // Ensure the folder exists
        if (!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
        }
        string filePath = Path.Combine(folderPath, "CompletedTasks.json");
        // Write the JSON to the file
        File.WriteAllText(filePath, json);
    }
}
