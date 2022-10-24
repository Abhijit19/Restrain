using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUI : MonoBehaviour
{
    public GameObject infoGroup;

    public GameObject roundOverGroup;
    public Text currentScoreText;
    public Text currentWinnerText;
    public Text roundCountDownText;

    public GameObject gameOverGroup;
    public Text reason;
    public Text scoreText;

    public GameObject levelTimerGroup;

    private void Start()
    {
        infoGroup.SetActive(true);
        gameOverGroup.SetActive(false);
        roundOverGroup.SetActive(false);
        levelTimerGroup.SetActive(false);
    }

    public void OnGameStart()
    {
        infoGroup.SetActive(false);
        gameOverGroup.SetActive(false);
        roundOverGroup.SetActive(false);
        levelTimerGroup.SetActive(true);
    }

    public void OnRoundEnd()
    {
        infoGroup.SetActive(false);
        gameOverGroup.SetActive(false);
        roundOverGroup.SetActive(true);
        levelTimerGroup.SetActive(false);
        SetRoundResult();
    }

    public void OnGameEnd()
    {
        infoGroup.SetActive(false);
        gameOverGroup.SetActive(true);
        roundOverGroup.SetActive(false);
        levelTimerGroup.SetActive(false);
        SetGameOverMessage();
    }

    public void SetGameOverMessage()
    {
        int attackerScore = 0;
        int defenderScore = 0;

        object attackerScoreObject = 0;
        object defenderScoreObject = 0;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RestrainGameManager.ATTACKER_SCORE_KEY, out attackerScoreObject))
            attackerScore = (int)attackerScoreObject;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RestrainGameManager.DEFENDER_SCORE_KEY, out defenderScoreObject))
            defenderScore = (int)defenderScoreObject;

        if(attackerScore > defenderScore)
        {
            reason.text = $"Attacker Won!";
        }
        else if (attackerScore < defenderScore)
        {
            reason.text = $"Defender Won!";
        }
        else
        {
            reason.text = $"Game Tied!";
        }

        scoreText.text = $"Attacker <b> {attackerScore} - {defenderScore} </b> Defender";
    }

    public void SetRoundResult()
    {
        int attackerScore = 0;
        int defenderScore = 0;

        object attackerScoreObject = 0;
        object defenderScoreObject = 0;
        object currentWinnerObject = null;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RestrainGameManager.ATTACKER_SCORE_KEY, out attackerScoreObject))
            attackerScore = (int)attackerScoreObject;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RestrainGameManager.DEFENDER_SCORE_KEY, out defenderScoreObject))
            defenderScore = (int)defenderScoreObject;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RestrainGameManager.ROUND_WINNER_KEY, out currentWinnerObject))
            currentWinnerText.text = $"{currentWinnerObject.ToString()} Won!";
        currentScoreText.text = $"Attacker <b> {attackerScore} - {defenderScore} </b> Defender";
    }
}
