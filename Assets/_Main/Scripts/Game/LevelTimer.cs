using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    private ServerTimer serverTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Update()
    {
        if (serverTimer != null)
            serverTimer.Tick(Time.deltaTime);
    }

    public void OnLevelStart()
    {
        serverTimer = new ServerTimer("Level1Timer", 10);
        serverTimer.OnTimerEnd += ServerTimer_OnTimerEnd;
        serverTimer.SetStartTime();
    }

    private void ServerTimer_OnTimerEnd()
    {
        Debug.Log("Timer End");
    }
}
