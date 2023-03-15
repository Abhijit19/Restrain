using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuestNameUI : MonoBehaviour
{
    public TMP_InputField NickNameInput;
    public const string NICKNAMEKEY = "nickname";

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        if(PlayerPrefs.HasKey(NICKNAMEKEY))
        {
            NickNameInput.text = PlayerPrefs.GetString(NICKNAMEKEY);
        }
        else
        {
            NickNameInput.text = "Player " + Random.Range(1000,9999);
        }
       
    }

    public void SetNickName(string NewNickName)
    {
        if ( NickNameInput.text.Trim().Length > 3)
        {
            NickNameInput.text = NewNickName;
            PlayerPrefs.SetString(NICKNAMEKEY, NickNameInput.text);
        }
    }


}
