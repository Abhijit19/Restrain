using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ServerTimer
{
    public float RemainingSeconds { get; private set; }
    public float Duration { get; private set; }

    public string TimerKey = "Timer";

    private int startTime;

    public ServerTimer(string key, float duration)
    {
        TimerKey = key;
        Duration = duration;
        RemainingSeconds = duration;
    }

    public event Action OnTimerEnd;

    public void Tick(float deltaTime)
    {
        // Stop ticking if the timer has already ended
        if (RemainingSeconds == 0f) { return; }

        RemainingSeconds = TimeRemaining();

        // Check for timer end
        CheckForTimerEnd();
    }

    private void CheckForTimerEnd()
    {
        // Leave if there is still time left to tick
        if (RemainingSeconds > 0f) { return; }

        // Set to zero due to duration possibly going below zero with the deltaTime subtraction
        RemainingSeconds = 0f;

        // Alert any listeners that the timer has ended
        OnTimerEnd?.Invoke();
    }

    public bool TryGetStartTime(out int startTimestamp)
    {
        startTimestamp = PhotonNetwork.ServerTimestamp;

        object startTimeFromProps;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(TimerKey, out startTimeFromProps))
        {
            startTimestamp = (int)startTimeFromProps;
            return true;
        }

        return false;
    }

    public void SetStartTime()
    {
        int startTimestamp = 0;
        bool wasSet = TryGetStartTime(out startTimestamp);

        if (wasSet)
        {
            Debug.Log("Using already set timer!");
            startTime = startTimestamp;
            return;
        }

        Debug.Log("Setting timer!");
        startTime = PhotonNetwork.ServerTimestamp;
        //Set the start time property
        Hashtable props = new Hashtable
            {
                {TimerKey,startTime}
            };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

    }

    private float TimeRemaining()
    {
        int timer = PhotonNetwork.ServerTimestamp - this.startTime;
        return this.Duration - timer / 1000f;
    }
}
