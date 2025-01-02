using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusScreenTutorialManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> StepLists;
    [SerializeField] private BarManager bar1;
    [SerializeField] private BarManager bar2;
    [SerializeField] private BarManager lightBar;

    private int _index;

    public void CycleThroughTutorial()
    {
        if (_index < StepLists.Count - 1)
        {
            StepLists[_index].SetActive(false);
            _index++;
            StepLists[_index].SetActive(true);
        }
        else
        {
            Debug.Log("Tutorial Completed");
        }

        switch (_index)
        {
            case 2:
                bar1.SetFillAmountValue(0);
                bar2.SetFillAmountValue(1);
                break;
            case 3:
                bar1.SetFillAmountValue(0.3f);
                bar2.SetFillAmountValue(0.65f);
                break;
            case 4:
                lightBar.SetFillAmountValue(0);
                break;
            case 5:
                lightBar.SetFillAmountValue(1);
                break;
            case 6:
                lightBar.SetFillAmountValue(0.27f);
                break;
            default:
                break;
        }
        
    }
}
