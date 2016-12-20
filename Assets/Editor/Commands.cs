using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LunarPlugin;

[CCommand("give", Values="mushroom")]
class Cmd_give : PlayerCommand
{
    void Execute(string item)
    {
        var player = GetPlayer();
        if (item == "mushroom")
        {
            player.AdvanceState();
        }
    }
}

class PlayerCommand : CCommand
{
    protected PlayerController GetPlayer()
    {
        return GameObject.FindObjectOfType<PlayerController>();
    }
}