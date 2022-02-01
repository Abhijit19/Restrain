using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInfoUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;

    public void OnItemEquip(Item item)
    {
        itemNameText.text = item.title;
    }
}
