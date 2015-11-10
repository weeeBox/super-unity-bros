﻿using UnityEngine;
using System.Collections;

public class TitleMenu : MonoBehaviour
{
    #region Button Handlers

    public void OnStartGameButton()
    {
        Application.LoadLevel(Scenes.Main);
    }

    public void OnStartTestButton()
    {
        Application.LoadLevel(Scenes.TestSelection);
    }

    #endregion
}
