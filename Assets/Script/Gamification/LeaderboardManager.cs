using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> EntryList;
    [SerializeField] private List<UserDataManager> UsersList;
    [SerializeField] private UserDataManager userData;
    
    private void SetLeaderboardEntry(GameObject entry ,int pos, string name, int points)
    {
        entry.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = pos.ToString();
        entry.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        entry.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = points.ToString();
    }

    private void SortLeaderboard()
    {
        UsersList = UsersList.OrderByDescending(obj => obj.GetTotalPoints()).ToList();
        for (int i = 0; i < 9; i++)
        {
            SetLeaderboardEntry(EntryList[i], i + 1, UsersList[i].GetName(), UsersList[i].GetTotalPoints());
        }
    }

    private void Start()
    {
        SortLeaderboard();
    }
}
