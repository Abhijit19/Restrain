using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public int damageAmount = 1;
    public bool stopCamera = false;
    public bool canDamage = true;

    private void Reset()
    {
        //GetComponent<Rigidbody>().isKinematic = true;
        //GetComponent<Collider>().isTrigger = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!canDamage)
            return;

        Debug.Log(collision.gameObject.name);
        var d = collision.gameObject.GetComponent<Damageable>();
        if (d == null)
            return;

        Debug.Log("Damage "+collision.gameObject.name);
        return;

        var msg = new Damageable.DamageMessage()
        {
            amount = damageAmount,
            damager = this,
            direction = Vector3.up,
            stopCamera = stopCamera
        };

        d.ApplyDamage(msg);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!canDamage)
            return;

        var d = other.GetComponent<Damageable>();
        if (d == null)
            return;

        Debug.Log(other.gameObject.name);

        var msg = new Damageable.DamageMessage()
        {
            amount = damageAmount,
            damager = this,
            direction = Vector3.up,
            stopCamera = stopCamera
        };

        d.ApplyDamage(msg);
    }
}
