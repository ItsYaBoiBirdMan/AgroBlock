using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskController : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI TaskName;
   [SerializeField] private TextMeshProUGUI TaskDesc;
   [SerializeField] private TextMeshProUGUI TimeLeft;
   [SerializeField] private TextMeshProUGUI Difficulty;
   [SerializeField] private Slider ProgressBar;

   private int _goal;
   [SerializeField] private int _progress;
   private int _pointReward;
   private Task _task;
   [SerializeField]private GameObject phantomTask;
   private GameObject instantiatedPhantomTask;

   public void SetTaskInfo( Task task)
   {
      _task = task;
      TaskName.text = "Name: " + task.GetTitle();
      TaskDesc.text = "Description: " + task.GetDesc();
      Difficulty.text = task.GetTaskDifficulty();
      _goal = task.GetProgressGoal();
      _pointReward = task.GetPointsReward();
      ProgressBar.maxValue = _goal;
       instantiatedPhantomTask = Instantiate(phantomTask);
      if (gameObject) {
         Debug.Log(gameObject);
         Debug.Log(TimeLeft);
         instantiatedPhantomTask.GetComponent<PhantomTaskController>().startPhantomTask(gameObject, task.GetTime() , true, TimeLeft);

      }
   }

   public GameObject getPhantomTask() {
      return instantiatedPhantomTask;
   }
   public Task getTask() {
      return _task;
   }
   

   public void AddProgressToTask()
   {
      int increment = Mathf.RoundToInt(_goal * (25 / 100));
      if (increment < 1) increment = 1;

      _progress += increment;

      ProgressBar.value = _progress;
      
      if (_progress >= _goal)
      {
         EventManager.RemoveCompletedTaskEvent.Invoke(gameObject);
      }
   }

   public (string, string) GetInfoForCompletedTask()
   {
      string name = TaskName.text.Replace("Name: ", "");
      string diff = Difficulty.text;

      return (name, diff);
   }

   public int GetReward()
   {
      return _pointReward;
   }
   
}
