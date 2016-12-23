using UnityEngine;
using System.Collections;

using LunarCore;

public class Mushroom : Powerup
{
    public override void Apply(PlayerController player)
    {
        player.AdvanceState();
    }
}
