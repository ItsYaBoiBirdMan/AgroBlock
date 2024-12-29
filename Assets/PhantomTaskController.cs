using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhantomTaskController : MonoBehaviour
{
    [SerializeField] private bool _timerStarted;
    [SerializeField] private float _totalTimeInSeconds;
    [SerializeField] private TextMeshProUGUI TimeLeft;
    [SerializeField] private GameObject task;
    private bool started;

    // Start is called before the first frame update
    public void startPhantomTask(GameObject taskGameObject,float timeInSeconds,bool timerStarted, TextMeshProUGUI textMeshProUGUI) {
        task = taskGameObject;
        _totalTimeInSeconds = timeInSeconds;
        _timerStarted = timerStarted;
        TimeLeft = textMeshProUGUI;
        started = true;
    }

    // Update is called once per frame
    private void Update(){
            if (_timerStarted && _totalTimeInSeconds > 0 && TimeLeft != null) {
                _totalTimeInSeconds -= Time.deltaTime;
                UpdateTimer();
         
                if (_totalTimeInSeconds <= 0) {
                    _totalTimeInSeconds = 0;
                    _timerStarted = false;
                    Debug.Log("Timer done");
                    UpdateTimer();
                    EventManager.RemoveTimeOutTaskEvent.Invoke(task);
                }
            }
    }
    
    private void UpdateTimer()
    {
        int hours = Mathf.FloorToInt(_totalTimeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((_totalTimeInSeconds % 3600) / 60);
        int seconds = Mathf.FloorToInt(_totalTimeInSeconds % 60);
      
        string timerFormatted = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        TimeLeft.text = "Time: " + timerFormatted;
    }
}
