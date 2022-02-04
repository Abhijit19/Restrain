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
        
        if (holding > 0)
        {
            
            Consume();
            //We we have more items, then use cooldown
            if (holding > 0)
                StartCooldown();
            else
                CanUse = false;
        }
    }

    public void Consume()
    {
        holding--;
        Damageable damageable = GetComponent<Damageable>();
        damageable.GainHealth(amount);
    }
}
