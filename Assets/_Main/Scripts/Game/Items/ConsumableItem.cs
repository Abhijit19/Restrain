using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableItem : Item, IPunObservable
{
    [Header("Consumable Item Info")]
    [SerializeField]
    protected int holdingCapacity = 4;
    [SerializeField]
    protected int holding = 0;
    public abstract override void Equip();

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(holding);
        }
        else
        {
            holding = (int)stream.ReceiveNext();
        }
    }

    public abstract override void Unequip();

    public abstract override void Use();

    public virtual void Add(int count = 1)
    {
        if (holding + count < holdingCapacity + 1)
            holding += count;
        else
            holding = holdingCapacity;
    }

    public int GetHolding()
    {
        return holding;
    }
}
