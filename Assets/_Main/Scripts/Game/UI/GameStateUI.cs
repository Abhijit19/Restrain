using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateUI : MonoBehaviour
{
    public GameObject infoGroup;
    public GameObject gameOverGroup;
    public GameObject levelTimerGroup;

    private void Start()
    {
        infoGroup.SetActive(true);
        gameOverGroup.SetActive(false);
        levelTimerGroup.SetActive(false);
    }

    public void OnGameStart()
    {
        infoGroup.SetActive(false);
        gameOverGroup.SetActive(false);
        levelTimerGroup.SetActive(true);
    }

    public void OnGameEnd()
    {
        infoGroup.SetActive(false);
        gameOverGroup.SetActive(true);
        levelTimerGroup.SetActive(false);
    }
}
