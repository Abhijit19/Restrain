using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.Events;

public class PlayerItemManager : MonoBehaviourPunCallbacks
{
    [Header("Usable Items")]
    public Item[] usableItems;
    private int itemIndex = -1;
    private int previousItemIndex = -1;

    public ItemEvent OnItemEquip;

    private void Awake()
    {
        usableItems = GetComponents<Item>();
        
    }

    private void Start()
    {
        if (usableItems.Length > 0)
        {
            EquipItem(0);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
        AbilityUpdate();
    }

    #region Abilities
    void AbilityUpdate()
    {
        //Equip Items
        for (int i = 0; i < usableItems.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (itemIndex >= usableItems.Length - 1)
                EquipItem(0);
            else
                EquipItem(itemIndex + 1);
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (itemIndex <= 0)
                EquipItem(usableItems.Length - 1);
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

    }
    #endregion

    [System.Serializable]
    public class ItemEvent : UnityEvent<Item>{}

}
