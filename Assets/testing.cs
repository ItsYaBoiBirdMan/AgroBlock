using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Script.StateMachine;
using Script.WifiConnection;
using UnityEngine;

public class testing : MonoBehaviour
{
    [SerializeField] private CropsLoader cropsLoader;

    
    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(4);
        Debug.Log("HEreD");
        cropsLoader.SelectSoilType("Black Sandy Loam");
        Debug.Log("SOIL TYPE SELECTED");
        
        cropsLoader.SelectCrop("Lettuce");
        
    }
    
    private void Start()
    {
        StartCoroutine(waiter());

    }
}
