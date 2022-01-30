using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableItem : Item
{
    public abstract override void Equip();

    public abstract override void Unequip();

    public abstract override void Use();
}
