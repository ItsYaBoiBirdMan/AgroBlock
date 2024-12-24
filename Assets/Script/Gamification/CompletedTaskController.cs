using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompletedTaskController : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI TaskName;
   [SerializeField] private TextMeshProUGUI Difficulty;

   [SerializeField] private TextMeshProUGUI TaskDesc;
   [SerializeField] private TextMeshProUGUI TimeLeft;
   

   private int _goal;
   private int _pointReward;
   private Task _task;

   public void SetTaskInfo( Task task)
   {
      _task = task;
      TaskName.text = "Name: " + task.GetTitle();
      Difficulty.text = "Difficulty: " + task.GetTaskDifficulty();
      _goal = task.GetProgressGoal();
      _pointReward = task.GetPointsReward();
   }
   public Task getTask() {
      return _task;
   }

 

}
