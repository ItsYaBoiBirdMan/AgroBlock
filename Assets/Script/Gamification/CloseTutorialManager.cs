using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject TutorialMenuHeader;
    [SerializeField] private GameObject TutorialMenuBody;
    [SerializeField] private GameObject Notif;
    [SerializeField] private UserDataManager User;


    private void ResetWhenTutorialClose()
    {
        TutorialMenuHeader.SetActive(true);
        TutorialMenuBody.SetActive(true);
        Notif.SetActive(true);
        
    }

    private void OnEnable()
    {
        EventManager.CloseTutorial.AddListener(ResetWhenTutorialClose);
        EventManager.GiveUserPointsAfterTutorial.AddListener(GiveUserPoints);
    }

    private void OnDisable()
    {
        EventManager.CloseTutorial.RemoveListener(ResetWhenTutorialClose);
        EventManager.GiveUserPointsAfterTutorial.RemoveListener(GiveUserPoints);
    }

    private void GiveUserPoints(int points)
    {
        User.AddPoints(points);
    }
    
}
