using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeScript : MonoBehaviour
{
    private bool isContentPanelActive;

    private void Update() {
        if (gameObject.activeSelf && !isContentPanelActive){
            isContentPanelActive = true;
            startSomething();
        }
        else if (!gameObject.activeSelf && isContentPanelActive) {
            isContentPanelActive = false;
        }
    }

    public void startSomething() {
        Debug.Log("Start something");
    }
}
