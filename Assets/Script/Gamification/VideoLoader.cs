using System;
using System.Collections;
using System.Collections.Generic;
using Script.Gamification.Tutorials;
using TMPro;
using UnityEngine;

public class VideoLoader : MonoBehaviour
{
    [SerializeField] private GameObject VideoHeader;
    [SerializeField] private GameObject VideoBody;
    [SerializeField] private GameObject TutorialHeader;
    [SerializeField] private GameObject TutorialBody;
    [SerializeField] private GameObject VideoPrefab;
    [SerializeField] private GameObject Footer;
    [SerializeField] private Transform VideoParent;

    [SerializeField] private Transform ButtonParent;

    public void LoadVideoTutorial()
    {
        Instantiate(VideoPrefab, VideoParent);
    }

    private void CloseTutorial()
    {
        foreach (Transform child in VideoParent)
        {
            Destroy(child.gameObject);
        }
        
        VideoHeader.SetActive(false);
        VideoBody.SetActive(false);
        TutorialHeader.SetActive(true);
        TutorialBody.SetActive(true);
        Footer.SetActive(true);
    }

    public void ShowButton()
    {
        foreach (Transform child in ButtonParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnEnable()
    {
        EventManager.CloseVideoTutorial.AddListener(CloseTutorial);
    }

    private void OnDisable()
    {
        EventManager.CloseVideoTutorial.RemoveListener(CloseTutorial);
    }
}
