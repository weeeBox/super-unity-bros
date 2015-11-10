using UnityEngine;
using System.Collections;

using LunarCore;

public class SceneEscape : BaseBehaviour
{
    [SerializeField]
    string backToScene;

    protected override void OnUpdate(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(backToScene);
        }
    }
}
