using System.Collections;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.Events;
using System;

public class RestrainGameManager : MonoBehaviourPunCallbacks
{
    public static RestrainGameManager Instance = null;

    public UnityEvent OnGameStart;
    public UnityEvent OnGameEnd;

    public UnityEvent OnRoundStart;
    public UnityEvent OnRoundEnd;

    public bool IsTesting = false;

    private SpawnPointHelper spawnPointHelper;
    private GameObject player;

    public const string GAMESTATE_KEY = "GameState";
    public const string ATTACKER_SCORE_KEY = "ATTACKER_SCORE";
    public const string DEFENDER_SCORE_KEY = "DEFENDER_SCORE";
    public const string ROUND_WINNER_KEY = "ROUND_WINNER";

    #region UNITY

    public void Awake()
    {
        Instance = this;
        spawnPointHelper = GetComponent<SpawnPointHelper>();
        
    }

    public override void OnEnable()
    {
        base.OnEnable();
        Debug.Log($"RestrainGameManager IsTimerRunning : {CountdownTimer.IsTimerRunning}");
        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        if (!CountdownTimer.IsTimerRunning)
            StartGame();
    }

    public void Start()
    {
        Hashtable props = new Hashtable
        {
            {Constants.PLAYERKEYS.PLAYER_LOADED_LEVEL, true}
            //,{Constants.PLAYERKEYS.HEALTH, 100}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("The player disconnected from server. Load the lobby scene");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckEndOfGame();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(Constants.PLAYERKEYS.HEALTH))
        {
            CheckEndOfGame();
            return;
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }


        // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
        int startTimestamp;
        bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

        if (changedProps.ContainsKey(Constants.PLAYERKEYS.PLAYER_LOADED_LEVEL))
        {
            if (CheckAllPlayerLoadedLevel())
            {
                if (!startTimeIsSet)
                {
                    CountdownTimer.SetStartTime();
                }
            }
            else
            {
                // not all players loaded yet. wait:
                Debug.Log("setting text waiting for players! ");
            }
        }

    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log("RestrainGameManager.OnRoomPropertiesUpdate " + propertiesThatChanged.ToStringFull());

        //If its a game state chaange, then check for end of game or round
        if (propertiesThatChanged.ContainsKey(GAMESTATE_KEY))
        {
            OnGameStateChange();
        }
    }

    #endregion

    // called by OnCountdownTimerIsExpired() when the timer ended
    private void StartGame()
    {
        Debug.Log("StartGame!");

        //Raise event
        OnGameStart.Invoke();

        //Start the round
        StartRound();
    }

    private void StartRound()
    {
        Debug.Log($"IsMasterClient : {PhotonNetwork.IsMasterClient}");
        Debug.Log("StartRound!");

        Transform spawn = spawnPointHelper.GetSpawnPoint(PhotonNetwork.LocalPlayer.ActorNumber - 1);
        Vector3 position = spawn.position;
        Quaternion rotation = spawn.rotation;

        //string operatorName = "DefaultPlayer";
        string operatorName = "AnimatedPlayerTest";                                                                     //MOD
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(Constants.PLAYERKEYS.OPERATOR))
        {
            operatorName = (string)PhotonNetwork.LocalPlayer.CustomProperties[Constants.PLAYERKEYS.OPERATOR];
            Debug.LogWarning($"Selected Operator {operatorName}");
        }

        //TODO: Update the player according to the player data
        player = PhotonNetwork.Instantiate($"PhotonPrefabs/{operatorName}", position, rotation, 0);
        //player.GetComponent<Damageable>()?.OnDeath.AddListener(OnDeath);

        //Raise event
        OnRoundStart.Invoke();
    }

    private void EndRound()
    {
        //Destroy the player object
        PhotonNetwork.Destroy(player);

        //Reset elemets

        //Start the round Timer

        //Raise event
        OnRoundEnd.Invoke();
    }

    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            Debug.Log("CheckAllPlayerLoadedLevel");
            object playerLoadedLevel;

            if (p.CustomProperties.TryGetValue(Constants.PLAYERKEYS.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
            {
                Debug.Log("CheckAllPlayerLoadedLevel :"+ playerLoadedLevel);
                if ((bool)playerLoadedLevel)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }

    private void CheckEndOfGame()
    {
        bool allDestroyed = true;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object health;
            if (p.CustomProperties.TryGetValue(Constants.PLAYERKEYS.HEALTH, out health))
            {
                if ((int)health > 0)
                {
                    allDestroyed = false;
                    break;
                }
            }
        }

        if (allDestroyed)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StopAllCoroutines();
            }

            string winner = "";
            int score = -1;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.GetScore() > score)
                {
                    winner = p.NickName;
                    score = p.GetScore();
                }
            }

        }
    }

    private void OnCountdownTimerIsExpired()
    {
        StartGame();
    }

    public void OnRoundCountdownTimerIsExpired()
    {
        StartRound();
    }

    public void OnDeath()
    {
        if (IsTesting)
        {
            Debug.LogWarning("Not destroying the player while testing");
            return;
        }
        Debug.Log("Player died!");
        GameEndReason("Attacker Won");
    }

    public  void OnLevelTimerFinish()
    {
        if (IsTesting)
        {
            Debug.LogWarning("Not destroying the player while testing");
            return;
        }
        Debug.Log("Timer ended!");
        GameEndReason("Attacker");
    }

    public void OnDefend()
    {
        if (IsTesting)
        {
            Debug.LogWarning("Not destroying the player while testing");
            return;
        }

        GameEndReason("Defender Won");
    }

    private void GameEndReason(string winner)
    {
        int attackerScore = 0;
        int defenderScore = 0;

        object attackerScoreObject = 0;
        object defenderScoreObject = 0;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(ATTACKER_SCORE_KEY, out attackerScoreObject))
            attackerScore = (int) attackerScoreObject;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(DEFENDER_SCORE_KEY, out defenderScoreObject))
            defenderScore = (int) defenderScoreObject;

        attackerScore++;

        Hashtable props = new Hashtable
        {
            {GAMESTATE_KEY, (int)2},
            {ATTACKER_SCORE_KEY, attackerScore},
            {DEFENDER_SCORE_KEY, defenderScore},
            {ROUND_WINNER_KEY, winner}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    private void OnGameStateChange()
    {
        int attackerScore = 0;
        int defenderScore = 0;

        object attackerScoreObject = 0;
        object defenderScoreObject = 0;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(ATTACKER_SCORE_KEY, out attackerScoreObject))
            attackerScore = (int)attackerScoreObject;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(DEFENDER_SCORE_KEY, out defenderScoreObject))
            defenderScore = (int)defenderScoreObject;

        if (attackerScore > 2 || defenderScore > 2)
        {
            OnGameOver();
        }
        else
        {
            EndRound();
        }
    }

    private void OnGameOver()
    {
        //Destroy the player object
        PhotonNetwork.Destroy(player);
        //Raise event
        OnGameEnd.Invoke();
    }

    public void ExitToLobby()
    {
        PhotonNetwork.LeaveRoom();
    }

}
