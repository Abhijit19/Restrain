using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.Events;

public class PlayerItemManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Usable Items")]
    public Item[] allItems;
    public List<Item> usableItems;
    private int itemIndex = -1;
    private int previousItemIndex = -1;

    public ItemEvent OnItemEquip;
    public ItemEvent OnItemUse;
    public ItemEvent OnItemPickedUp;

    private void Awake()
    {
        allItems = GetComponents<Item>();
        
    }

    private void Start()
    {
        if (usableItems.Count > 0)
        {
            EquipItem(0);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
        ItemUpdate();
    }

    #region Abilities
    void ItemUpdate()
    {
        if (usableItems.Count == 0)
            return;

        //Equip Items
        for (int i = 0; i < usableItems.Count; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (itemIndex >= usableItems.Count - 1)
                EquipItem(0);
            else
                EquipItem(itemIndex + 1);
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (itemIndex <= 0)
                EquipItem(usableItems.Count - 1);
            else
                EquipItem(itemIndex - 1);
        }

        //Check for user input
        bool use = false;
        if (Input.GetButton("Fire1") && usableItems[itemIndex].IsContinuesUse)
            use = true;
        else if (Input.GetButtonDown("Fire1"))
            use = true;

        //Check for fire input
        if (use && usableItems[itemIndex].CanUse)
        {
            if (!PhotonNetwork.IsConnected)
            {
                RPC_TriggerAbility(itemIndex);
                return;
            }
            //m_Abilities[0].TriggerAbility();
            //Debug.Log("Fire :"+Time.time);
            photonView.RPC("RPC_TriggerAbility", RpcTarget.All, itemIndex);
        }
    }

    private void EquipItem(int itemIndex)
    {
        //If we are changing to the same item, then skip it
        if (this.itemIndex == itemIndex)
            return;
        //If we have a previous item already equiped, then unequip
        if (previousItemIndex != -1)
            usableItems[previousItemIndex].Unequip();
        //equip the selected item
        usableItems[itemIndex].Equip();
        this.itemIndex = itemIndex;
        this.previousItemIndex = itemIndex;

        //make local player to send itemIndex
        if (photonView.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add(Constants.PLAYERKEYS.SELECTEDITEM, itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            OnItemEquip.Invoke(usableItems[itemIndex]);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!photonView.IsMine && targetPlayer == photonView.Owner && changedProps.ContainsKey(Constants.PLAYERKEYS.SELECTEDITEM))
        {
            EquipItem((int)changedProps[Constants.PLAYERKEYS.SELECTEDITEM]);
        }
    }

    [PunRPC]
    public void RPC_TriggerAbility(int index)
    {
        //Debug.Log("RPC Fire :" + Time.time);
        usableItems[index].Use();

        //raise event 
        if (photonView.IsMine)
            OnItemUse.Invoke(usableItems[index]);

    }
    #endregion

    #region PICKUP

    [PunRPC]
    public void RPC_OnItemPickUp(string itemName)
    {
        Item pickedItem = FindItem(itemName);
        if (!pickedItem)
        {
            Debug.LogWarning("Invalid item picked up " + itemName);
            return;
        }
        //If we picked a consumable item
        if (pickedItem is ConsumableItem)
        {
            Debug.Log("Consumable");
            ((ConsumableItem)pickedItem).Add(1);
        }

        if (usableItems.Contains(pickedItem))
        {
            Debug.Log("Already contains item");
        }
        else
        {
            usableItems.Add(pickedItem);
            if (usableItems.Count == 1)
                EquipItem(0);
        }

        //Raise event
        if(photonView.IsMine)
            OnItemPickedUp.Invoke(pickedItem);
    }
    public bool OnItemPickUp(string itemName)
    {
        if (!photonView.IsMine)
            return true;

        Item pickedItem = FindItem(itemName);
        if (!pickedItem)
        {
            Debug.LogWarning("Invalid item picked up "+itemName);
            return false;
        }

        if (!PhotonNetwork.IsConnected)
        {
            RPC_OnItemPickUp(itemName);
            return true;
        }
        //m_Abilities[0].TriggerAbility();
        //Debug.Log("Fire :"+Time.time);
        photonView.RPC("RPC_OnItemPickUp", RpcTarget.All, itemName);
        return true;
    }

    private Item FindItem(string itemName)
    {
        Item found = null;
        for (int i = 0; i < allItems.Length; i++)
        {
            if (allItems[i].title.Equals(itemName, System.StringComparison.InvariantCultureIgnoreCase))
            {
                found = allItems[i];
                break;
            }
        }
        return found;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(usableItems);
        }
        else
        {
            usableItems = (List<Item>)stream.ReceiveNext();
        }
    }
    #endregion

    [System.Serializable]
    public class ItemEvent : UnityEvent<Item>{}

}
