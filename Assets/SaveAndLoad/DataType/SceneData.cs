using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    // stats
    public string sceneName;
    public float[] positionOnScene;

    public SceneData(string sceneName, Vector2 positionOnScene)
    {
        this.sceneName = sceneName;
        this.positionOnScene = new float[2];
        this.positionOnScene[0] = positionOnScene.x;
        this.positionOnScene[1] = positionOnScene.y;
    }
}
