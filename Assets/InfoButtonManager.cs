using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject CropButtonWindow;
    [SerializeField] private GameObject SoilButtonWindow;
    [SerializeField] private GameObject GrowthStageButtonWindow;
    [SerializeField] private StartScript startScript;

    public void ToggleCropWindow()
    {
        if (!CropButtonWindow.activeInHierarchy) {
            CropButtonWindow.SetActive(true);
            SetCropWindowText(startScript.Crop.Name);
        }
        else CropButtonWindow.SetActive(false);
    }
    
    public void ToggleSoilWindow()
    {
        if (!SoilButtonWindow.activeInHierarchy)
        {
            SoilButtonWindow.SetActive(true);
            SetSoilWindowText(startScript.Crop.Soils[0].Type);
        }
        else SoilButtonWindow.SetActive(false);
    }
    
    public void ToggleGrowthStageWindow()
    {
        if (!GrowthStageButtonWindow.activeInHierarchy)
        {
            GrowthStageButtonWindow.SetActive(true);
            int i = startScript.CurrentStage;
            SetGrowthStageWindowText(startScript.Crop.Soils[0].GrowthStages[i].Stage);
        }
        else GrowthStageButtonWindow.SetActive(false);
    }

    private void SetCropWindowText(string cropName)
    {
        CropButtonWindow.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "You are growing: " + cropName;
    }
    
    private void SetSoilWindowText(string soilName)
    {
        SoilButtonWindow.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "You are using " + soilName + " soil";
    }
    
    private void SetGrowthStageWindowText(string growthStageName)
    {
        GrowthStageButtonWindow.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "The crop is in the " + growthStageName + " stage";
    }

}