
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class TaskScreenManager : MonoBehaviour
{
    [SerializeField] private Transform TaskParent;
    [SerializeField] private Transform CompletedTaskParent;
    [SerializeField] private List<GameObject> CurrentTasks;
    [SerializeField] private List<GameObject> CompletedTasks;
    [SerializeField] private List<Task> completedTasks;
    [SerializeField] private GameObject TaskPrefab;
    [SerializeField] private GameObject CompletedTaskPrefab;
    [SerializeField] private Button deleteButton;


    private void CreateNewTask(string taskName, string taskDesc, float totalTimeInSecs, string diff, int goal, int reward)
    {
        var newTask = Instantiate(TaskPrefab, TaskParent);
        newTask.GetComponent<TaskController>().SetTaskInfo(taskName, taskDesc, totalTimeInSecs, diff, goal, reward);
        CurrentTasks.Add(newTask);
    }

    private void Start() {
        string inputFilePath = "CompletedTasks.json"; // Ensure the correct JSON filename
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "JSON", inputFilePath);
        if (!File.Exists(jsonFilePath)){
            completedTasks = new List<Task>();
        } else {
            try {
                completedTasks = JsonConvert.DeserializeObject<List<Task>>(File.ReadAllText(jsonFilePath));
            } catch (Exception ex) {
                Debug.LogError($"Failed to load crops: {ex.Message}");
            } 
        }
        CurrentTasks = new List<GameObject>();
        CompletedTasks = new List<GameObject>();
        DisplayCompletedTasks(completedTasks);
        deleteButton.onClick.AddListener(DeleteCompletedTasks);
    }

    public void TestTaskCreation()
    {
        CreateNewTask("Test", "Test Desc", 10f, "Easy", 10, 1000);
    }

    private void CreateCompletedTask(string taskName, string diff) {
        var compTask = Instantiate(CompletedTaskPrefab, CompletedTaskParent);
        compTask.GetComponent<CompletedTaskController>().SetTaskInfo(taskName, diff);
        CompletedTasks.Add(compTask);
        Task task = ScriptableObject.CreateInstance<Task>();
        task.SetTitle(taskName);
        task.SetTaskDifficulty(diff);
        completedTasks.Add(task);
        SaveCompletedTasksIntoFile(completedTasks);
        DisplayCompletedTasks(completedTasks);
    }

    private void RemoveTaskOnCompletion(GameObject task)
    {
        CurrentTasks.Remove(task);
        CreateCompletedTask(task.GetComponent<TaskController>().GetInfoForCompletedTask().Item1, task.GetComponent<TaskController>().GetInfoForCompletedTask().Item2);
        Destroy(task);
    }

    private void RemoveTaskOnTimeOut(GameObject task)
    {
        CurrentTasks.Remove(task);
        Destroy(task);
    }

    public void SaveCompletedTasksIntoFile(List<Task> tasks){
        string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
        string folderPath = Path.Combine(Application.persistentDataPath, "JSON");
        if (!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
        }
        string filePath = Path.Combine(folderPath, "CompletedTasks.json");
        File.WriteAllText(filePath, json);
    }
    public void DisplayCompletedTasks(List<Task> tasks){
        for (var i = CompletedTaskParent.transform.childCount - 1; i >= 0; i--) {
            Destroy(CompletedTaskParent.transform.GetChild(i).gameObject);
        }
        foreach (Task task in tasks){
            var newTask = Instantiate(CompletedTaskPrefab, CompletedTaskParent);
            newTask.GetComponent<CompletedTaskController>().SetTaskInfo(task.GetTitle(), task.GetTaskDifficulty());
            CompletedTasks.Add(newTask);
        }
    }

    public void DeleteCompletedTasks() {
        completedTasks = new List<Task>();
        SaveCompletedTasksIntoFile(completedTasks);
        DisplayCompletedTasks(completedTasks);
    }
    
    private void OnEnable()
    {
        EventManager.RemoveCompletedTaskEvent.AddListener(RemoveTaskOnCompletion);
        EventManager.RemoveTimeOutTaskEvent.AddListener(RemoveTaskOnTimeOut);
    }

    private void OnDisable()
    {
        EventManager.RemoveCompletedTaskEvent.RemoveListener(RemoveTaskOnCompletion);
        EventManager.RemoveTimeOutTaskEvent.RemoveListener(RemoveTaskOnTimeOut);
    }
    
    
}
