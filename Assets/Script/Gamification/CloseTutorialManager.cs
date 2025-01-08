using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject TutorialMenu;
    [SerializeField] private GameObject Notif;


    private void ResetWhenTutorialClose()
    {
        TutorialMenu.SetActive(true);
        Notif.SetActive(true);
    }

    private void OnEnable()
    {
        EventManager.CloseTutorial.AddListener(ResetWhenTutorialClose);
    }

    private void OnDisable()
    {
        EventManager.CloseTutorial.RemoveListener(ResetWhenTutorialClose);
    }
}
