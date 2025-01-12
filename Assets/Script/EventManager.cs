using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
   public static UnityEvent<Task> TaskCompletedEvent = new UnityEvent<Task>();
   public static UnityEvent<string> SendNotification = new UnityEvent<string>();

   public static UnityEvent<GameObject> RemoveCompletedTaskEvent = new UnityEvent<GameObject>();
   public static UnityEvent<GameObject> RemoveTimeOutTaskEvent = new UnityEvent<GameObject>();

   public static UnityEvent CloseTutorial = new UnityEvent();
   public static UnityEvent<int> GiveUserPointsAfterTutorial = new UnityEvent<int>();

}
