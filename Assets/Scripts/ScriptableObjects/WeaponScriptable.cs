using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon", order = 2)]
public class WeaponScriptable : EquippableScriptable
{
    public WeaponStats weaponStats;

    public override void UseItem(PlayerController playerController)
    {
        if (Equipped)
        {
            // Unequip from Inventory and Controller
            playerController.weaponHolder.UnquipWeapon();
        } else
        {
            // invvoke on weapon from player events here for inventory
            // equip weapon from weapon holder on player controller
            playerController.weaponHolder.UnquipWeapon();
            playerController.weaponHolder.EquipWeapon(this);
            //PlayerEvents.InvokeOnWeaponEquipped(itemPrefab.GetComponent<WeaponComponent>());


        }

        base.UseItem(playerController);
    }
}
