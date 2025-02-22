using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserDataManager : MonoBehaviour
{
    [SerializeField] private int totalPoints;
    [SerializeField] private int currentPoints;
    [SerializeField] private string userName;

    public void AddPoints(int points)
    {
        totalPoints += points;
        currentPoints += points;
    }
    public void SpendPoints(int points){
        currentPoints -= points;
    }
    public int GetTotalPoints()
    {
        return totalPoints;
    }
    public int GetCurrentPoints()
    {
        return currentPoints;
    }

    public string GetName()
    {
        return userName;
    }
}
