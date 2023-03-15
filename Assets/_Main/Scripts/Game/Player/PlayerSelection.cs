using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public string PlayerPath = "PhotonPrefabs";
    public GameObject[] Availableplayers = null;
    public const string SELECTEDPLAYERKEY = "SELECTEDPLAYERINDEX";
    public int SelectedPlayerIndex = 0;
    public GameObject selected = null;

    private void Start()
    {
        Availableplayers = Resources.LoadAll<GameObject>(PlayerPath);
        SelectedPlayerIndex = PlayerPrefs.GetInt(SELECTEDPLAYERKEY, 0);
        if (Availableplayers != null)
        {
            selected = GameObject.Instantiate(Availableplayers[SelectedPlayerIndex],new Vector3(), Quaternion.Euler(0,180,0),transform);
        }
    }

    public void Next()
    {
        if (Availableplayers == null || Availableplayers.Length == 0)
            return;

        SelectedPlayerIndex++;
        SelectedPlayerIndex = SelectedPlayerIndex % Availableplayers.Length;

        SetPlayer();

    }

    public void Previous()
    {
        if (Availableplayers == null || Availableplayers.Length == 0)
            return;

        SelectedPlayerIndex--;
        if(SelectedPlayerIndex < 0)
            SelectedPlayerIndex = Availableplayers.Length - 1;

        SetPlayer();
    }

    private void SetPlayer()
    {
        PlayerPrefs.SetInt(SELECTEDPLAYERKEY, SelectedPlayerIndex);
        PlayerPrefs.Save();

        if(selected != null)
        {
            GameObject.Destroy(selected);
            selected = null;
        }

        if (Availableplayers != null)
        {
            selected = GameObject.Instantiate(Availableplayers[SelectedPlayerIndex], Vector3.zero, Quaternion.Euler(0, 180, 0), transform);
        }
    }
}
