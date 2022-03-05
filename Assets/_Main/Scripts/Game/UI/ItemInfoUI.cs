using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInfoUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    private Item current = null;
    private Coroutine coroutine;
    public void OnItemPickedUp(Item item)
    {
        if (current != null)
            OnItemEquip(current);
    }

    public void OnItemEquip(Item item)
    {
        current = item;
        itemNameText.text = item.title;

        if (item is ConsumableItem)
        {
            Debug.Log("Consumable");
            itemNameText.text += $":{((ConsumableItem)item).GetHolding()}";
        }
        else
        {
            itemNameText.text += $":~";
        }
    }

    public void OnItemUse(Item item)
    {
        StartCoroutine(Cooldown());
        IEnumerator Cooldown()
        {
            yield return new WaitForEndOfFrame();
            if (current != null)
                OnItemEquip(current);
        }
    }
}
