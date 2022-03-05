using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void onclickplayerselect(int whichchar)
    {
        if (PlayerInfo.playerinfo != null)
        {
            PlayerInfo.playerinfo.selectedCharacter = whichchar;
            PlayerPrefs.SetInt("Mychar", whichchar);
        }
    }

   
}
