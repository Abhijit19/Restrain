using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : ConsumableItem
{
    [Header("Health Item Info")]
    public int amount = 40;
    public override void Equip()
    {
    }

    public override void Unequip()
    {
    }

    public override void Use()
    {
        //Check if we can use
        if (!CanUse)
            return;
        
        //if (holding > 0)
        {
            StartCooldown();
            Consume();
        }
    }

    public void Consume()
    {
        holding--;
        Damageable damageable = GetComponent<Damageable>();
        damageable.GainHealth(amount);
    }
}
