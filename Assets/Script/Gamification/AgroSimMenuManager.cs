using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgroSimMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject SimPrefab;
    [SerializeField] private Transform SimParent;
    
    [SerializeField] private GameObject SimMenuHeader;
    [SerializeField] private GameObject SimMenuBody;
    [SerializeField] private GameObject SimHeader;
    [SerializeField] private GameObject SimBody;
    [SerializeField] private GameObject Footer;

    public void StartAgroSim()
    {
        Instantiate(SimPrefab, SimParent);
    }

    private void CloseAgroSim()
    {
        SimHeader.SetActive(false);
        SimBody.SetActive(false);
        SimMenuHeader.SetActive(true);
        SimMenuBody.SetActive(true);
        Footer.SetActive(true);
        
        foreach (Transform child in SimParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnEnable()
    {
        EventManager.CloseAgroSimEvent.AddListener(CloseAgroSim);
    }

    private void OnDisable()
    {
        EventManager.CloseAgroSimEvent.RemoveListener(CloseAgroSim);
    }
}
