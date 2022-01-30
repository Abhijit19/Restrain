using System.Collections;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.Events;

public class RestrainGameManager : MonoBehaviourPunCallbacks
{
    public static RestrainGameManager Instance = null;

    public UnityEvent OnGameStart;

    private SpawnPointHelper spawnPointHelper;

    #region UNITY

    public void Awake()
    {
        Instance = this;
        spawnPointHelper = GetComponent<SpawnPointHelper>();
        
    }

    public override void OnEnable()
    {
        base.OnEnable();
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
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

    #endregion

    // called by OnCountdownTimerIsExpired() when the timer ended
    private void StartGame()
    {
        Debug.Log("StartGame!");
        
        // on rejoin, we have to figure out if the spaceship exists or not
        // if this is a rejoin (the ship is already network instantiated and will be setup via event) we don't need to call PN.Instantiate

        Transform spawn = spawnPointHelper.GetSpawnPoint(PhotonNetwork.LocalPlayer.ActorNumber-1);
        Vector3 position = spawn.position;
        Quaternion rotation = spawn.rotation;

        string operatorName = "DefaultPlayer";
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(Constants.PLAYERKEYS.OPERATOR))
            operatorName = (string)PhotonNetwork.LocalPlayer.CustomProperties[Constants.PLAYERKEYS.OPERATOR];
        
        //TODO: Update the player according to the player data
        GameObject player = PhotonNetwork.Instantiate($"PhotonPrefabs/{operatorName}", position, rotation, 0);      // avoid this call on rejoin (ship was network instantiated before)
        
        //Raise event
        OnGameStart.Invoke();
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
}
