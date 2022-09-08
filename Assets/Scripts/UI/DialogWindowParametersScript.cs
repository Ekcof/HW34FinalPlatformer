using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogWindowParametersScript : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool dialogIsActive;
    private int messagesInQueue;
    public int GetMessagesInQueue()
    {
        return messagesInQueue;
    }
    public void AddMessageInQueue()
    {
        messagesInQueue++;
    }

    public void RemoveMessageInQueue()
    {
        messagesInQueue--;
        if (messagesInQueue < 0) messagesInQueue = 0;
    }
    public bool CheckActive()
    {
        return dialogIsActive;
    }

    public void ChangeStatus()
    {
        dialogIsActive = !dialogIsActive;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
