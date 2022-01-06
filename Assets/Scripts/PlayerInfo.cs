using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    public static PlayerInfo playerinfo;

    public int selectedCharacter;
    public GameObject[] allchars;
    private void OnEnable()
    {
        if(PlayerInfo.playerinfo==null)
        {
            PlayerInfo.playerinfo = this;
        }
        else
        {
            if (PlayerInfo.playerinfo != this)
            {
                Destroy(PlayerInfo.playerinfo.gameObject);
                PlayerInfo.playerinfo = this;
            }
        }
       // DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("Mychar"))
        {
            selectedCharacter = PlayerPrefs.GetInt("Mychar");
        }
        else
        {
            selectedCharacter = 0;
            PlayerPrefs.SetInt("Mychar", selectedCharacter);
        }
    }

    
    
}
