using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UsableItem : MonoBehaviour
{
    [System.Serializable]
    public class OnItemUseEvent : UnityEvent<float> { }
    public OnItemUseEvent OnItemUse = new OnItemUseEvent();

    [Header("Usable Item Info")]
    public string title;
    public Sprite icon;
    public float cooldownTime = 1;
    private bool canUse = true;
    public bool CanUse { get { return canUse; } }

    public void UseItem()
    {
        if (canUse)
        {
            OnItemUse.Invoke(cooldownTime);
            Use();
            StartCooldown();
        }

    }
    public abstract void Use();

    public abstract void Equip();

    public abstract void Unequip();
    void StartCooldown()
    {
        StartCoroutine(Cooldown());
        IEnumerator Cooldown()
        {
            canUse = false;
            yield return new WaitForSeconds(cooldownTime);
            canUse = true;
        }
    }
}
