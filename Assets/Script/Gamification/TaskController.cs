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
   

   public void SetTaskInfo(string taskName, string taskDesc, string timeLeft, string diff, int goal, int reward)
   {
      TaskName.text = "Name: " + taskName;
      TaskDesc.text = "Description: " + taskDesc;
      TimeLeft.text = "Time Left: " + timeLeft;
      Difficulty.text = diff;
      _goal = goal;
      _pointReward = reward;

      ProgressBar.maxValue = _goal;
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
   
}
