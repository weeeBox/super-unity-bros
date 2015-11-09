using UnityEngine;
using System.Collections;

public class GoombaController : EnemyController
{
    public override void OnPlayerJump(MarioController player)
    {
        base.OnPlayerJump(player);
        Squash();
    }

    void Squash()
    {
        Die(true);
    }
}
