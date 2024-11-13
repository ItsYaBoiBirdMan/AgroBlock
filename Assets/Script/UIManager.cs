using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Task System")] 
    
    [SerializeField] private GameObject TaskMenu;
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private List<GameObject> tasksText; 
    [SerializeField]private List<Task> currentTasks;
    
    [Header("Main Menu")] 
    
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject SideMenu;
    [SerializeField] private Transform SideMenuShowing;
    [SerializeField] private Transform SideMenuHidden;
    [SerializeField] private List<Button> ButtonsToBeDisabled;
    [SerializeField] private GameObject CloseSideMenuButton;

    [Header("Screen Positions")] 
    
    [SerializeField] private Transform MiddleOfScreen;
    [SerializeField] private Transform OutsideLeft;
    [SerializeField] private Transform OutsideRight;
    
    
    private bool _shouldSideMenuSlideIn;
    private bool _shouldSideMenuSlideOut;

    private bool _shouldTaskMenuSlideIn;
    private bool _shouldTaskMenuSlideOut;


    private void UpdateDisplayedTasks()
    {
        for (int i = 0; i < tasksText.Count; i++)
        {
            if(!currentTasks[i]) continue;
            tasksText[i].GetComponent<Text>().text = currentTasks[i].GetTitle();
            tasksText[i].GetComponent<Text>().color = Color.white;
            tasksText[i].transform.GetChild(0).GetComponent<Slider>().maxValue = currentTasks[i].GetProgressGoal();
            tasksText[i].transform.GetChild(0).GetComponent<Slider>().value = currentTasks[i].GetCurrentProgress();
            tasksText[i].transform.GetChild(1).GetComponent<Text>().text = currentTasks[i].GetDesc();
        }
    }

    private void UpdateProgressBar()
    {
        for (int i = 0; i < tasksText.Count; i++)
        {
            if(!currentTasks[i]) continue;
            tasksText[i].transform.GetChild(0).GetComponent<Slider>().value = currentTasks[i].GetCurrentProgress();
        }
    }

    private void RemoveDisplayedTaskWhenCompleted(int index)
    {
        tasksText[index].GetComponent<Text>().text = "Task Completed";
        tasksText[index].GetComponent<Text>().color = Color.gray;
    }
    public void SimulateTaskProgress(int index)
    {
        if (currentTasks[index] != null)
        {
            var addedProgress = Mathf.RoundToInt(currentTasks[index].GetProgressGoal() * (25 / 100f));
            if (addedProgress < 1) addedProgress = 1;
            currentTasks[index].AddToCurrentProgressValue(addedProgress);
            UpdateDisplayedTasks();
            if (currentTasks[index].GetCurrentProgress() == currentTasks[index].GetProgressGoal()) RemoveDisplayedTaskWhenCompleted(index);
            UpdateProgressBar();
        }
        else Debug.Log("That Task is already completed");
    }

    public void GetNewTasks()
    {
        currentTasks = new List<Task>();
        taskManager.RefreshTasks();
        currentTasks = taskManager.GetCurrentTasks();
        UpdateDisplayedTasks();
        Debug.Log("Refresh");
    }

   
    private void HandleSideMenuMovement()
    {
        if (_shouldSideMenuSlideIn)
        {
            SideMenu.transform.position =
                Vector2.Lerp(SideMenu.transform.position, SideMenuShowing.position, 10 * Time.deltaTime);
            
            if (Vector2.Distance(SideMenu.transform.position, SideMenuShowing.position) < 0.1f)
            {
                SideMenu.transform.position = SideMenuShowing.position;
                _shouldSideMenuSlideIn = false; 
            }
            
        }
        
        if (_shouldSideMenuSlideOut)
        {
            SideMenu.transform.position =
                Vector2.Lerp(SideMenu.transform.position, SideMenuHidden.position, 10 * Time.deltaTime);
            
            if (Vector2.Distance(SideMenu.transform.position, SideMenuHidden.position) < 0.1f)
            {
                SideMenu.transform.position = SideMenuHidden.position;
                _shouldSideMenuSlideOut = false; 
            }
            
        }
    }

    private void HandleTaskMenuMovement()
    {
        if (_shouldTaskMenuSlideIn)
        {
            MainMenu.transform.position =
                Vector2.Lerp(MainMenu.transform.position, OutsideLeft.position, 10 * Time.deltaTime);
            TaskMenu.transform.position =
                Vector2.Lerp(TaskMenu.transform.position, MiddleOfScreen.position, 10 * Time.deltaTime);
            
            if (Vector2.Distance(TaskMenu.transform.position, MiddleOfScreen.position) < 0.1f)
            {
                TaskMenu.transform.position = MiddleOfScreen.position;
                MainMenu.transform.position = OutsideLeft.position;
                _shouldTaskMenuSlideIn = false; 
            }
            
        }
        
        if (_shouldTaskMenuSlideOut)
        {
            MainMenu.transform.position =
                Vector2.Lerp(MainMenu.transform.position, MiddleOfScreen.position, 10 * Time.deltaTime);
            TaskMenu.transform.position =
                Vector2.Lerp(TaskMenu.transform.position, OutsideRight.position, 10 * Time.deltaTime);
            
            if (Vector2.Distance(MainMenu.transform.position, MiddleOfScreen.position) < 0.1f)
            {
                TaskMenu.transform.position = OutsideRight.position;
                MainMenu.transform.position = MiddleOfScreen.position;
                _shouldTaskMenuSlideOut = false; 
            }
            
        }
    }
    
    public void StartSlidingIn()
    {
        _shouldSideMenuSlideIn = true;
        _shouldSideMenuSlideOut = false;
        CloseSideMenuButton.SetActive(true);
        for (int i = 0; i < ButtonsToBeDisabled.Count; i++)
        {
            ButtonsToBeDisabled[i].interactable = false;
        }
    }

    public void StartSlidingOut()
    {
        _shouldSideMenuSlideIn = false;
        _shouldSideMenuSlideOut = true;
        CloseSideMenuButton.SetActive(false);
        for (int i = 0; i < ButtonsToBeDisabled.Count; i++)
        {
            ButtonsToBeDisabled[i].interactable = true;
        }
    }

    public void StartSlidingInTaskMenu()
    {
        _shouldTaskMenuSlideIn = true;
        _shouldSideMenuSlideOut = false;
        StartSlidingOut();
    }

    public void StartSlidingOutTaskMenu()
    {
        _shouldTaskMenuSlideIn = false;
        _shouldTaskMenuSlideOut = true;
        CloseSideMenuButton.SetActive(true);
    }

    public void Test()
    {
        Debug.Log("Test");
    }
    
    private void Start()
    {
        currentTasks = new List<Task>();
        currentTasks = taskManager.GetCurrentTasks();
        
        UpdateDisplayedTasks();
    }

    private void Update()
    {
        HandleSideMenuMovement();
        HandleTaskMenuMovement();
    }
}