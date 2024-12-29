
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Button = UnityEngine.UI.Button;

public class TaskScreenManager : MonoBehaviour
{
    [SerializeField] private Transform TaskParent;
    [SerializeField] private Transform CompletedTaskParent;
    [SerializeField] private List<GameObject> CurrentTasks;
    [SerializeField] private List<Task> activeTasks;
    [SerializeField] private List<GameObject> CompletedTasks;
    private List<Task> completedTasks;
    [SerializeField] private List<Task> allTasks;
    
    [SerializeField] private GameObject TaskPrefab;
    [SerializeField] private GameObject CompletedTaskPrefab;
    [SerializeField] private Button deleteButton;


    private void CreateNewTask(List<Task> tasks) {
        foreach (Task task in tasks) {
            var newTask = Instantiate(TaskPrefab, TaskParent);
            newTask.GetComponent<TaskController>().SetTaskInfo(task);
            CurrentTasks.Add(newTask);
        }
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
        RefreshCurrentTasks(false);
        deleteButton.onClick.AddListener(DeleteCompletedTasks);
    }



    private void CreateCompletedTask(Task task) {
        var compTask = Instantiate(CompletedTaskPrefab, CompletedTaskParent);
        compTask.GetComponent<CompletedTaskController>().SetTaskInfo(task);
        CompletedTasks.Add(compTask);
        completedTasks.Add(task);
        SaveCompletedTasksIntoFile(completedTasks);
        DisplayCompletedTasks(completedTasks);
    }

    private void RemoveTaskOnCompletion(GameObject task) {
        if (activeTasks.Contains(task.GetComponent<TaskController>().getTask())) {
            activeTasks.Remove(task.GetComponent<TaskController>().getTask());
        }
        
        CurrentTasks.Remove(task);
        //create completed task and add it to both lists
        CreateCompletedTask(task.GetComponent<TaskController>().getTask());
        GameObject phantomTask= task.GetComponent<TaskController>().getPhantomTask();
        Destroy(phantomTask);
        Destroy(task);
        
    }

    private void RemoveTaskOnTimeOut(GameObject task)
    {
        CurrentTasks.Remove(task);
        GameObject phantomTask= task.GetComponent<TaskController>().getPhantomTask();
        Destroy(phantomTask);
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
            newTask.GetComponent<CompletedTaskController>().SetTaskInfo(task);
            CompletedTasks.Add(newTask);
        }
    }

    public void DeleteCompletedTasks() {
        completedTasks = new List<Task>();
        SaveCompletedTasksIntoFile(completedTasks);
        DisplayCompletedTasks(completedTasks);
        RefreshCurrentTasks(true);
    }

    public void RefreshCurrentTasks(bool refreshAll) {
        List<Task> temp = new List<Task>();
        if (!refreshAll) {
            foreach (Task task in allTasks) {
                temp.Add(task);
                Debug.Log("I got here");
                if (completedTasks.Count== 0 ) {
                    activeTasks.Add(task);
                }
                foreach (var completed in completedTasks) {
                    if (completed.GetTitle().Equals(task.GetTitle())|| activeTasks.Contains(task)){
                        temp.Remove(task);
                    }
                }
            } 
            activeTasks = temp;
        } else {
            foreach (Task task in allTasks){
                if (!activeTasks.Contains(task)){
                    activeTasks.Add(task);
                    temp.Add(task);
                }
            }
        }
        CreateNewTask(temp);
        
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
