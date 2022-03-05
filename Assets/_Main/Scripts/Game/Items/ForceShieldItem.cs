using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceShieldItem : ConsumableItem
{
    [Header("ForceShield Item Info")]
    public GameObject shieldModel;
    public float shieldTime = 10;
    private Coroutine coroutine;
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
            shieldModel.SetActive(true);
            yield return new WaitForSeconds(shieldTime);
            shieldModel.SetActive(false);
        }
    }
}
