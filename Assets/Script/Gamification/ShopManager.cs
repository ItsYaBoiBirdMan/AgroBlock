using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI UserPoints;
    [SerializeField] private UserDataManager UserData;
    [SerializeField] private GameObject NotEnoughPointsWarning;

    public void UpdateOnOpening()
    {
        UserPoints.text = "Points: " + UserData.GetCurrentPoints();
    }

    public void Purchase(int price)
    {
        if (UserData.GetCurrentPoints() >= price)
        {
            UserData.SpendPoints(price);
            UpdateOnOpening();
        }
        else
        {
            StartCoroutine(StartNotEnoughPointsWarning());
        }
    }


    private IEnumerator StartNotEnoughPointsWarning()
    {
        NotEnoughPointsWarning.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        NotEnoughPointsWarning.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        NotEnoughPointsWarning.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        NotEnoughPointsWarning.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        NotEnoughPointsWarning.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        NotEnoughPointsWarning.SetActive(false);
    }
}
