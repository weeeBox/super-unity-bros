using UnityEngine;
using System.Collections;

public class TitleMenu : MonoBehaviour
{
    #region Button Handlers

    public void StartGame()
    {
        Application.LoadLevel("Main");
    }

    public void StartTest()
    {
        // TODO
    }

    #endregion
}
