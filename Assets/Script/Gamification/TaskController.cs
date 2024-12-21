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
   [SerializeField]private float _totalTimeInSeconds;

   private bool _timerStarted;
   

   public void SetTaskInfo(string taskName, string taskDesc, float totalTimeInSecs, string diff, int goal, int reward)
   {
      TaskName.text = "Name: " + taskName;
      TaskDesc.text = "Description: " + taskDesc;
      Difficulty.text = diff;
      _goal = goal;
      _pointReward = reward;
      _totalTimeInSeconds = totalTimeInSecs;
      ProgressBar.maxValue = _goal;

      _timerStarted = true;
   }

   private void UpdateTimer()
   {
      int hours = Mathf.FloorToInt(_totalTimeInSeconds / 3600);
      int minutes = Mathf.FloorToInt((_totalTimeInSeconds % 3600) / 60);
      int seconds = Mathf.FloorToInt(_totalTimeInSeconds % 60);
      
      string timerFormatted = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
      TimeLeft.text = "Time: " + timerFormatted;
   }

   public void AddProgressToTask()
   {
      int increment = Mathf.RoundToInt(_goal * (25 / 100));
      if (increment < 1) increment = 1;

      _progress += increment;

      ProgressBar.value = _progress;
      
      if (_progress >= _goal)
      {
         EventManager.RemoveTaskEvent.Invoke(gameObject);
      }
   }

   private void Update()
   {
      if (_timerStarted && _totalTimeInSeconds > 0)
      {
         _totalTimeInSeconds -= Time.deltaTime;
         UpdateTimer();
         
         if (_totalTimeInSeconds <= 0)
         {
            _totalTimeInSeconds = 0;
            _timerStarted = false;
            Debug.Log("Timer done");
            UpdateTimer();
            EventManager.RemoveTaskEvent.Invoke(gameObject);
         }
      }
   }
}
