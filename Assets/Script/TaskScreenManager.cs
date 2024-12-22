
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class TaskScreenManager : MonoBehaviour
{
    [SerializeField] private Transform TaskParent;
    [SerializeField] private Transform CompletedTaskParent;
    [SerializeField] private List<GameObject> CurrentTasks;
    [SerializeField] private List<GameObject> CompletedTasks;
    [SerializeField] private GameObject TaskPrefab;
    [SerializeField] private GameObject CompletedTaskPrefab;


    private void CreateNewTask(string taskName, string taskDesc, float totalTimeInSecs, string diff, int goal, int reward)
    {
        var newTask = Instantiate(TaskPrefab, TaskParent);
        newTask.GetComponent<TaskController>().SetTaskInfo(taskName, taskDesc, totalTimeInSecs, diff, goal, reward);
        CurrentTasks.Add(newTask);
    }

    private void Start()
    {
        CurrentTasks = new List<GameObject>();
        CompletedTasks = new List<GameObject>();
    }

    public void TestTaskCreation()
    {
        CreateNewTask("Test", "Test Desc", 10f, "Easy", 10, 1000);
    }

    private void CreateCompletedTask(string taskName, string diff)
    {
        var completedTask = Instantiate(CompletedTaskPrefab, CompletedTaskParent);
        completedTask.GetComponent<CompletedTaskController>().SetTaskInfo(taskName, diff);
        CompletedTasks.Add(completedTask);
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
