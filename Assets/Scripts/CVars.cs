using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LunarPlugin;

[CVarContainer]
public static class CVars
{
    public static CVar g_playerWalkSpeed        = new CVar("g_playerWalkSpeed", 48.0f);
    public static CVar g_playerRunSpeed         = new CVar("g_playerRunSpeed", 72.0f);

    public static CVar g_enemySpeed             = new CVar("g_enemySpeed", 12.0f);
    public static CVar g_powerupSpeed           = new CVar("g_powerupSpeed", 24.0f);
    public static CVar g_bulletBillSpeed        = new CVar("g_bulletBillSpeed", 36.0f);

    public static CVar g_piranhaPlantSpeed      = new CVar("g_piranhaPlantSpeed", 12.0f);
    public static CVar g_piranhaPlantIdleTime   = new CVar("g_piranhaPlantIdleTime", 1.0f);
    public static CVar g_piranhaPlantHiddenTime = new CVar("g_piranhaPlantHiddenTime", 1.25f);
}
