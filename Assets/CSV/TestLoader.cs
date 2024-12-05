using System.Collections;
using System.Collections.Generic;
using Script.StateMachine;
using UnityEngine;

public class TestLoader : MonoBehaviour
{
    [SerializeField] private CropsLoader cropsLoader;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(waiter());
        
    }
    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(10);
        Debug.Log("Show time");
        yield return new WaitForSecondsRealtime(2);
        cropsLoader.GenerateTestValues();
        
        
    }
}
