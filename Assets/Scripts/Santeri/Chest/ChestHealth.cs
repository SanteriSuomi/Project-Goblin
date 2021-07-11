using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestHealth : Chest
{
    public override void OnUse(PlayerHealth player)
    {
        float missingHealth = 100 - player.Health;
        player.ModifyHealth(missingHealth);
    }
}
