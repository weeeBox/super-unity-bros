using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

[CCommand("restart")]
class Cmd_restart : CPlayModeCommand
{
    void Execute()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

class PlayerCommand : CPlayModeCommand
{
    protected PlayerController GetPlayer()
    {
        return GameObject.FindObjectOfType<PlayerController>();
    }
}