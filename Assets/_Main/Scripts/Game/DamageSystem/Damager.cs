using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public int damageRate = 1;      //Damage per second
    public bool stopCamera = false;
    public bool canDamage = true;

    private void Reset()
    {
        //GetComponent<Rigidbody>().isKinematic = true;
        //GetComponent<Collider>().isTrigger = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckForDamage(collision.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        CheckForDamage(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForDamage(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckForDamage(other.gameObject);
    }


    private float lastDamageTime = 0;
    private float damageDelay = 1;

    private void CheckForDamage(GameObject other)
    {
        if (!canDamage)
            return;

        var d = other.GetComponent<Damageable>();
        if (d == null)
            return;

        ApplyDamage(d, damageRate);
    }

    private void ApplyDamage(Damageable d, int damageAmount)
    {
        //Damage delay check
        if (Time.time - lastDamageTime < damageDelay)
            return;

        lastDamageTime = Time.time;

        Debug.Log($"Damage {d.gameObject.name}, amount {damageAmount}");

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
