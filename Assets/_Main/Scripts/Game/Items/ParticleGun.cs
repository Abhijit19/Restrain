using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGun : Gun
{
    [Header("Connections")]
    [SerializeField] ParticleSystem particle = default;
    [SerializeField] GameObject gunModel;

    public override void Equip()
    {
        gunModel.SetActive(true);
    }

    public override void Unequip()
    {
        gunModel.SetActive(false);
    }

    public override void Use()
    {
        if (CanUse)
        {
            OnItemUse.Invoke(cooldownTime);
            Shoot();
            StartCooldown();
        }
        particle.Play();
    }

    private void Shoot()
    {

    }
}
