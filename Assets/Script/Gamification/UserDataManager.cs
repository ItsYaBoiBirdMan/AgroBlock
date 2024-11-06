using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserDataManager : MonoBehaviour
{
    [SerializeField] private int totalPoints;
    [SerializeField] private int currentPoints;

    public void AddPoints(int points)
    {
        totalPoints += points;
        currentPoints += points;
    }
}
