using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class TitleMenu : MonoBehaviour
{
    #region Button Handlers

    public void OnStartGameButton()
    {
        SceneManager.LoadScene(Scenes.World_1_1);
    }

    public void OnStartTestButton()
    {
    }

    #endregion
}
