using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] GameObject model;
    [Header("Usable Item Info")]
    public float fireRate = 0.1f;
    public int clipSize = 30;
    public int reserrvedAmmoCapacity = 270;

    int currentAmmoInClip;
    int ammoInReserve;

    public override void Equip()
    {
        model.SetActive(true);
    }

    public override void Unequip()
    {
        model.SetActive(false);
    }

    public override void Use()
    {
        if (CanUse)
        {
            OnItemUse.Invoke(cooldownTime);
            Shoot();
            StartCooldown();
        }
    }

    public virtual void Shoot()
    {

    }
}
