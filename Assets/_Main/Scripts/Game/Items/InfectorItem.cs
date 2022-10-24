using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
            //If not the owener, skip
            PhotonView view = GetComponent<PhotonView>();
            if (!view.IsMine)
                yield break;

            InfectorSpawnManager manager = FindObjectOfType<InfectorSpawnManager>();
            GameObject infector = manager.SpawnInfector();

            TankMovement tank = infector.GetComponent<TankMovement>();
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

            manager.RemoveInfector();
        }
    }
}
