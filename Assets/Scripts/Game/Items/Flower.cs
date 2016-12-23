using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Powerup
{
    public override void Apply(PlayerController player)
    {
        player.AdvanceState();
    }
}
