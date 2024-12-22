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

   

   public void SetTaskInfo(string taskName, string diff) {
      TaskName.text = "Name: " + taskName;
      Difficulty.text = "Difficulty: " + diff;
   }

}
