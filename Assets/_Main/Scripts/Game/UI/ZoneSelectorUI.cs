using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneSelectorUI : MonoBehaviour
{
    public Button[] zones;
    public ColorBlock normalColor;
    public ColorBlock selectedColor;
    private void Start()
    {
        if (zones == null || zones.Length == 0)
            return;

        normalColor = zones[0].colors;
        for (int i = 0; i < zones.Length; i++)
        {
            int current = i;
            zones[i].onClick.AddListener(()=>OnButtonSelected(zones[current]));
        }
    }

    private void OnButtonSelected(Button selected)
    {
        for (int i = 0; i < zones.Length; i++)
        {
            zones[i].GetComponent<Image>().color = Color.white;
        }

        selected.GetComponent<Image>().color = Color.blue;
    }
}
