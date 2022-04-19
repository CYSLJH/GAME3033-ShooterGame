using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquippableScriptable : ItemScript
{
    public bool Equipped
    {
        get => isEquipped;
        set
        {
            isEquipped = value;
            OnEquipStatusChange?.Invoke();
        }
    }

    bool isEquipped = false;

    public delegate void EquipStatusChange();
    public event EquipStatusChange OnEquipStatusChange;

    public override void UseItem(PlayerController playerController)
    {
        Equipped = !Equipped;
    }

}
