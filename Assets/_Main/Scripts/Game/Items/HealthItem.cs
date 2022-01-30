using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : ConsumableItem
{
    public int amount = 40;
    public override void Equip()
    {
    }

    public override void Unequip()
    {
    }

    public override void Use()
    {
        //Check if we have health left to consume
        //Do consume health
        Damageable damageable = GetComponent<Damageable>();
        damageable.GainHealth(amount);
    }
}
