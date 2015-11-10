using UnityEngine;
using System.Collections;

public class TestSelection : MonoBehaviour
{
    #region Button Handlers

    public void OnTestButton(int index)
    {
        Application.LoadLevel("Test-" + index);
    }

    public void OnBackButton()
    {
        Application.LoadLevel(Scenes.TestSelection);
    }

    #endregion
}
