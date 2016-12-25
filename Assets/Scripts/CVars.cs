using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LunarPlugin;

[CVarContainer]
public static class CVars
{
    public static CVar g_playerWalkSpeed        = new CVar("g_playerWalkSpeed", 35.0f);
    public static CVar g_playerWalkAcc          = new CVar("g_playerWalkAcceleration", 52.0f);
    public static CVar g_playerWalkBreakAcc     = new CVar("g_playerWalkBreakAcc", 150.0f);

    public static CVar g_playerRunSpeed         = new CVar("g_playerRunSpeed", 58.0f);
    public static CVar g_playerRunAcc           = new CVar("g_playerRunAcceleration", 76.0f);
    public static CVar g_playerRunBreakAcc      = new CVar("g_playerRunBreakAcc", 174.0f);

    public static CVar g_playerGravity          = new CVar("g_playerGravity", -455.0f);
    public static CVar g_playerGravityLongJump  = new CVar("g_playerGravityLongJump", -174.0f);
    public static CVar g_playerJumpSpeed        = new CVar("g_jumpSpeed", 96.0f);

    public static CVar g_enemySpeed             = new CVar("g_enemySpeed", 12.0f);
    public static CVar g_powerupSpeed           = new CVar("g_powerupSpeed", 24.0f);
    public static CVar g_bulletBillSpeed        = new CVar("g_bulletBillSpeed", 36.0f);

    public static CVar g_piranhaPlantSpeed      = new CVar("g_piranhaPlantSpeed", 12.0f);
    public static CVar g_piranhaPlantIdleTime   = new CVar("g_piranhaPlantIdleTime", 1.0f);
    public static CVar g_piranhaPlantHiddenTime = new CVar("g_piranhaPlantHiddenTime", 1.25f);
}
