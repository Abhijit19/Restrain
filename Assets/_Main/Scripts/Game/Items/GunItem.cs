using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : UsableItem
{
    [Header("Usable Item Info")]
    public float fireRate = 0.1f;
    public int clipSize = 30;
    public int reserrvedAmmoCapacity = 270;

    bool canShoot = true;
    int currentAmmoInClip;
    int ammoInReserve;

    public override void Equip()
    {
    }

    public override void Unequip()
    {
        
    }

    public override void Use()
    {
    }
}
