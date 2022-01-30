using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Item : MonoBehaviour
{
    [System.Serializable]
    public class OnItemUseEvent : UnityEvent<float> { }
    public OnItemUseEvent OnItemUse = new OnItemUseEvent();

    [Header("Usable Item Info")]
    public string title;
    public Sprite icon;
    public float cooldownTime = 1;
    [SerializeField]
    private bool isContinuesUse = false;
    private bool canUse = true;

    public bool CanUse { get { return canUse; } }
    public bool IsContinuesUse { get { return isContinuesUse; } }

    public abstract void Use();

    public abstract void Equip();

    public abstract void Unequip();

    public void StartCooldown()
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
