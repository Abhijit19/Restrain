using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectorItem : ConsumableItem
{
    private Coroutine coroutine;
    public float abilityTime = 60;
    public override void Equip()
    {
    }

    public override void Unequip()
    {
    }

    public override void Use()
    {
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
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Cooldown());
        IEnumerator Cooldown()
        {
            TankMovement tank = GameObject.FindObjectOfType<TankMovement>();
            if (tank != null)
            {
                GetComponent<PlayerController>().Deactivate();
                tank.Activate();
            }
            yield return new WaitForSeconds(abilityTime);
            if (tank != null)
            {
                GetComponent<PlayerController>().Activate();
                tank.Deactivate();
            }
        }
    }
}
