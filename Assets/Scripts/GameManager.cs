using UnityEngine;
using NotificationSamples;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameNotificationsManager gameNotificationsManager;
    [SerializeField] private int notificationDelay;

    private void Start()
    {
        InitializeNotificationChannel();
        CreateNotification();
    }

    private void InitializeNotificationChannel()
    {
        GameNotificationChannel channel = new GameNotificationChannel("mntutorial", "Mobile Notifications Tutorial", "Just a notification");
        gameNotificationsManager.Initialize(channel);
    }

    /*
    public void OnTimeInput(string text)
    {
        if (int.TryParse(text, out int sec))
            notificationDelay = sec;
    }
    */

    public void CreateNotification()
    {
        CreateNotification("Mobile Notifications Tutorial", "Come back to the game",
        DateTime.Now.AddSeconds(notificationDelay));
    }

    private void CreateNotification(string title, string body, DateTime time)
    {
        IGameNotification notification = gameNotificationsManager.CreateNotification();
        if (notification != null)
        {
            notification.Title = title;
            notification.Body = body;
            notification.DeliveryTime = time;
            gameNotificationsManager.ScheduleNotification(notification);
        }
    }
}
