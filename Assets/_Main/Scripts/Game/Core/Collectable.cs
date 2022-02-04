using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string itemName = string.Empty;
    public GameObject model;
    public ParticleSystem particle;

    public void OnTriggerEnter(Collider other)
    {

        PlayerItemManager itemManager = other.GetComponent<PlayerItemManager>();
        if (!itemManager)
            return;
        bool pickedUp = itemManager.OnItemPickUp(itemName);
        Debug.Log($"{gameObject.name} - {other.gameObject.name} - {pickedUp}");
        //We have picked up the item
        if (pickedUp)
        {
            model?.SetActive(false);
            particle?.Stop();

            Destroy(gameObject, 5);
        }
    }
}
