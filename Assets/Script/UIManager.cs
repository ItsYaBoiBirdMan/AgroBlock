using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Transform OutsideBottom;
    [SerializeField] private Transform OutsideTop;

    [Header("Leaderboard")] 
    
    [SerializeField] private GameObject Leaderboard;
    [SerializeField] private GameObject EntryPrefab;
    [SerializeField] private Transform EntryParent;
    [SerializeField] private List<GameObject> EntryList;
    [SerializeField] private UserDataManager userData;
    [SerializeField] private List<UserDataManager> usersList;
    
    private bool _shouldSideMenuSlideIn;
    private bool _shouldSideMenuSlideOut;

    private bool _shouldTaskMenuSlideIn;
    private bool _shouldTaskMenuSlideOut;

    private bool _shouldLeaderboardSlideIn;
    private bool _shouldLeaderboardSlideOut;

    //Task System
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
        SortLeaderboard();
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

   //Leaderboard
   
   private void SetLeaderboardEntry(GameObject entry ,int pos, string name, int points)
   {
       entry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pos.ToString();
       entry.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = name;
       entry.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = points.ToString();
   }

   private void SortLeaderboard()
   {
       usersList = usersList.OrderByDescending(obj => obj.GetTotalPoints()).ToList();
       for (int i = 0; i < 10; i++)
       {
           SetLeaderboardEntry(EntryList[i], i + 1, usersList[i].GetName(), usersList[i].GetTotalPoints());
       }
   }
    //UI Movement
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
    
    private void HandleLeaderboardMovement()
    {
        if (_shouldLeaderboardSlideIn)
        {
            MainMenu.transform.position =
                Vector2.Lerp(MainMenu.transform.position, OutsideTop.position, 10 * Time.deltaTime);
            Leaderboard.transform.position =
                Vector2.Lerp(Leaderboard.transform.position, MiddleOfScreen.position, 10 * Time.deltaTime);
            
            if (Vector2.Distance(Leaderboard.transform.position, MiddleOfScreen.position) < 0.1f)
            {
                Leaderboard.transform.position = MiddleOfScreen.position;
                MainMenu.transform.position = OutsideTop.position;
                _shouldLeaderboardSlideIn = false; 
            }
            
        }
        
        if (_shouldLeaderboardSlideOut)
        {
            MainMenu.transform.position =
                Vector2.Lerp(MainMenu.transform.position, MiddleOfScreen.position, 10 * Time.deltaTime);
            Leaderboard.transform.position =
                Vector2.Lerp(Leaderboard.transform.position, OutsideBottom.position, 10 * Time.deltaTime);
            
            if (Vector2.Distance(MainMenu.transform.position, MiddleOfScreen.position) < 0.1f)
            {
                Leaderboard.transform.position = OutsideBottom.position;
                MainMenu.transform.position = MiddleOfScreen.position;
                _shouldLeaderboardSlideOut = false; 
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
    
    public void StartSlidingInLeaderboard()
    {
        _shouldLeaderboardSlideIn = true;
        _shouldLeaderboardSlideOut = false;
        StartSlidingOut();
        SortLeaderboard();
    }

    public void StartSlidingOutLeaderboard()
    {
        _shouldLeaderboardSlideIn = false;
        _shouldLeaderboardSlideOut = true;
        CloseSideMenuButton.SetActive(true);
    }

    public void Test()
    {
        Debug.Log("Test");
    }

    private void Awake()
    {
        //EntryList = new List<GameObject>();
        float entryDistance = 80f;
        usersList = usersList.OrderByDescending(obj => obj.GetTotalPoints()).ToList();
        for (int i = 0; i < 10; i++)
        {
            EntryList[i] = Instantiate(EntryPrefab, EntryParent);
            EntryList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -entryDistance * i, 0);
            SetLeaderboardEntry(EntryList[i], i + 1, usersList[i].GetName(), usersList[i].GetTotalPoints());
        }
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
        HandleLeaderboardMovement();
    }
}