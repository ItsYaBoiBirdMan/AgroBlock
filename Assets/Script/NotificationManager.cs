using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> NotifHistory;
    [SerializeField] private GameObject NotifPrefab;
    [SerializeField] private GameObject NotifMenu;
    [SerializeField] private Transform NotifParent;
    [SerializeField] private Button NotifButton;
    public void DisplayNotifMessage(string message)
    {
        var notif = Instantiate(NotifPrefab, NotifParent);
        notif.GetComponent<TextMeshProUGUI>().text = message;
        NotifHistory.Add(notif);

        StartCoroutine(MakeNotifButtonBlink());
    }

    public void ClearNotifHistory()
    {
        NotifHistory.Clear();
        foreach (Transform child in NotifParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void ToggleNotifMenu()
    {
        if(!NotifMenu.activeInHierarchy) NotifMenu.SetActive(true);
        else NotifMenu.SetActive(false);
    }
    private void Start()
    {
        NotifHistory = new List<GameObject>();
    }

    private IEnumerator MakeNotifButtonBlink()
    {
        ColorBlock cb = NotifButton.colors;
        Color baseColor = new Color(0.9496855f, 0.9496855f, 0.9496855f);
        /////////
        cb.normalColor = Color.red;
        NotifButton.colors = cb;
        yield return new WaitForSeconds(.25f);
        cb.normalColor = baseColor;
        NotifButton.colors = cb;
        yield return new WaitForSeconds(.25f);
        cb.normalColor = Color.red;
        NotifButton.colors = cb;
        yield return new WaitForSeconds(.25f);
        cb.normalColor = baseColor;
        NotifButton.colors = cb;

    }

    private void OnEnable()
    {
        EventManager.SendNotification.AddListener(DisplayNotifMessage);
    }

    private void OnDisable()
    {
        EventManager.SendNotification.RemoveListener(DisplayNotifMessage);
    }
}
