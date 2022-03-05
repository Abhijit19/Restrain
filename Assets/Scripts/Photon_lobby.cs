using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Photon_lobby : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public Text ourroom,name_;
    public List<RoomInfo> roomlistings;
    public static Photon_lobby lobby;
    public string room_name;
    public int room_size;
    public GameObject roomlistingprefab;
    public Transform roompanel;
   // public GameObject battleButton,cancelbutton;
    // Start is called before the first frame update
    private void Awake()
    {
        lobby = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        roomlistings = new List<RoomInfo>();
    }

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        Debug.Log("player has connected to the master server.");
        PhotonNetwork.AutomaticallySyncScene = true;
    
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        //  removerooms();
        int tempindex;
        foreach(RoomInfo room in roomList)
        {
            if (roomlistings != null)
            {
                tempindex = roomlistings.FindIndex(ByName(room.Name));
            }
            else
            {
                tempindex = -1; 
            }
            if (tempindex != -1)
            {
                roomlistings.RemoveAt(tempindex);
                Destroy(roompanel.GetChild(tempindex).gameObject);
            }
            else
            {
                roomlistings.Add(room);
                listrooms(room);
            }
            
        }


    }
    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    void removerooms()
    {
        int i = 0;
        while (roompanel.childCount != 0)
        {
            Destroy(roompanel.GetChild(i).gameObject);
            i++;
        }
    }
    void listrooms(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible) {
        
            GameObject temproomlist = Instantiate(roomlistingprefab, roompanel);
            RoomButton roomButton = temproomlist.GetComponent<RoomButton>();
            roomButton.room_Name = room.Name;
            roomButton.room_siZe = room.MaxPlayers;
            roomButton.setroom();
        }

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed.Please try again.Maybe with different roomdetails ");
    }
    

    public  void CreateRoom()
    {
        Debug.Log("New room created");
        RoomOptions roomOps = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 5 };
        PhotonNetwork.CreateRoom(room_name, roomOps);
        
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public  void onroomnamechanged()
    {
        
        room_name =name_.text ;
        //Debug.Log(room_name);
    }
    //public void onroomsizechanged()
    //{
        
        
    //}

    public void joinlobbyonclick()
    {

    if(!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    
}
