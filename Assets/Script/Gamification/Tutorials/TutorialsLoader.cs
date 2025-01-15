using System;
using System.Collections;
using System.Collections.Generic;
using Script.Gamification.Tutorials;
using TMPro;
using UnityEngine;

public class TutorialsLoader : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPrefab;
    [SerializeField] private List<Tutorial> tutorials;
    [SerializeField] private Transform contentHolder;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject notification;
    [SerializeField] private Transform uiPanel;

    [SerializeField] private GameObject header;
    [SerializeField] private GameObject body;
    

    public void OnClickTry(){
        foreach (Transform child in contentHolder) {
            Destroy(child.gameObject);
        }
        foreach (Tutorial tutorial in tutorials) {
            
            // Instantiate a clone of the prefab
            GameObject clone = Instantiate(tutorialPrefab, contentHolder);

            var scriptableObjectHandler = clone.GetComponent<TutorialHandler>();
            if (scriptableObjectHandler != null){
                scriptableObjectHandler.SetTutorial(tutorial, menu, notification,uiPanel);
            } else {
                Debug.LogWarning("The prefab does not have a ScriptableObjectHandler component!");
            }
            
            TextMeshProUGUI textChild = clone.GetComponent<TextMeshProUGUI>();
            if (textChild != null) {
                textChild.text = tutorial.GetName(); // Update with the tutorial's name or other property
            } else {
                Debug.LogWarning("The prefab does not contain a TextMeshProUGUI child!");
            }
        }
    }
    
    public void CompleteOnClickTry(){
        foreach (Transform child in contentHolder) {
            Destroy(child.gameObject);
        }
        foreach (Tutorial tutorial in tutorials) {
            
            // Instantiate a clone of the prefab
            GameObject clone = Instantiate(tutorialPrefab, contentHolder);

            var scriptableObjectHandler = clone.GetComponent<TutorialHandler>();
            if (scriptableObjectHandler != null){
                scriptableObjectHandler.CompleteSetTutorial(tutorial, header, body, notification, uiPanel);
            } else {
                Debug.LogWarning("The prefab does not have a ScriptableObjectHandler component!");
            }
            
            TextMeshProUGUI textChild = clone.GetComponent<TextMeshProUGUI>();
            if (textChild != null) {
                textChild.text = tutorial.GetName(); // Update with the tutorial's name or other property
            } else {
                Debug.LogWarning("The prefab does not contain a TextMeshProUGUI child!");
            }
        }
    }
    
    
}
