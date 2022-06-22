using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUI : MonoBehaviour
{
    public GameObject infoGroup;
    public GameObject gameOverGroup;
    public Text reason;
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
        SetGameOverMessage();
    }

    public void SetGameOverMessage()
    {
        object reasonFromProps;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Reason", out reasonFromProps))
        {
            reason.text = $"Game Over! {reasonFromProps.ToString()}";
        }
    }
}
