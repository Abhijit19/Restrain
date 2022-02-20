using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneSelectorUI : MonoBehaviour
{
    public Button[] zones;
    public ColorBlock normalColor;
    public ColorBlock selectedColor;

    public string LevelSelecetd = "LEVEL_SELECTED";

    public PhotonRoom photonRoom;
    private void Start()
    {
        if (zones == null || zones.Length == 0)
            return;

        normalColor = zones[0].colors;
        for (int i = 0; i < zones.Length; i++)
        {
            int current = i;
            zones[i].onClick.AddListener(()=>OnButtonSelected(current));
        }

        int selected = PlayerPrefs.GetInt(LevelSelecetd, 0);
        OnButtonSelected(selected);
    }

    private void OnButtonSelected(int selected)
    {
        PlayerPrefs.SetInt(LevelSelecetd, selected);
        PlayerPrefs.Save();

        photonRoom.OnZoneSelected(selected);

        for (int i = 0; i < zones.Length; i++)
        {
            zones[i].GetComponent<Image>().color = Color.white;
        }

        zones[selected].GetComponent<Image>().color = Color.blue;
    }
}
