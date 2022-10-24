using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class WinButton : InteractableBase
{
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnInteract();
        }
    }
#endif

    public override void OnInteract()
    {
        Debug.Log("INTERACTED: " + gameObject.name);
        //Add win logic here?
        GameEndReason("Defender");
    }

    private void GameEndReason(string winner)
    {
        int attackerScore = 0;
        int defenderScore = 0;

        object attackerScoreObject = 0;
        object defenderScoreObject = 0;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RestrainGameManager.ATTACKER_SCORE_KEY, out attackerScoreObject))
            attackerScore = (int)attackerScoreObject;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RestrainGameManager.DEFENDER_SCORE_KEY, out defenderScoreObject))
            defenderScore = (int)defenderScoreObject;

        defenderScore++;

        Hashtable props = new Hashtable
        {
            {RestrainGameManager.GAMESTATE_KEY, (int)2},
            {RestrainGameManager.ATTACKER_SCORE_KEY, attackerScore},
            {RestrainGameManager.DEFENDER_SCORE_KEY, defenderScore},
            {RestrainGameManager.ROUND_WINNER_KEY, winner}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
}
