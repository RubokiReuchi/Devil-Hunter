using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MainMenuSaveAndLoad : MonoBehaviour
{
    [HideInInspector] public string targetSceneName;

    public void NewGame()
    {
        targetSceneName = "Tutorial Path";
        Vector2 positionOnScene = new(0.0f, -0.8f);

        SaveSystem.SaveScene(targetSceneName, positionOnScene);
    }
}
