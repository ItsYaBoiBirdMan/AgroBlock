using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject TutorialMenuHeader;
    [SerializeField] private GameObject TutorialMenuBody;
    [SerializeField] private GameObject Notif;


    private void ResetWhenTutorialClose()
    {
        TutorialMenuHeader.SetActive(true);
        TutorialMenuBody.SetActive(true);
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
