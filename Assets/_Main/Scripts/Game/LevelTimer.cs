using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    [Header("Reference to a Text component for visualizing the countdown")]
    public Text Text;
    public string timerMessage = "Time left";
    public float duration = 60;
    private ServerTimer serverTimer;

    public UnityEvent OnTimerFinish;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Update()
    {
        if (serverTimer != null)
        {
            serverTimer.Tick(Time.deltaTime);
            //Debug.Log(serverTimer.RemainingSeconds);
            Text.text = string.Format("{0} : {1:#0.00}", timerMessage, serverTimer.RemainingSeconds);
        }
    }

    public void OnLevelStart()
    {
        Debug.Log("Starting Timer");
        serverTimer = new ServerTimer("Level1Timer", duration);
        serverTimer.OnTimerEnd += ServerTimer_OnTimerEnd;
        serverTimer.SetStartTime();
        
    }

    private void ServerTimer_OnTimerEnd()
    {
        Debug.Log("Timer End");
        OnTimerFinish.Invoke();
    }
}
