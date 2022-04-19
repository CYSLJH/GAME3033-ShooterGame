using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/HealthConsumable", order = 1)]
public class HealthConsumableScriptable : ConsumableScriptable
{
    public override void UseItem(PlayerController playerController)
    {
        if (playerController.healthComponent.CurrentHealth >= playerController.healthComponent.MaxHealth) return;

        playerController.healthComponent.Heal(effect);
        base.UseItem(playerController);
    }
}
