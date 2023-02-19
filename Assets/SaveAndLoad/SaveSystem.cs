using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region Player
    static string dantePath = Application.persistentDataPath + "/dante.data";
    static string danteNewGamePath = Application.persistentDataPath + "/danteDefault.data";

    public static void SavePlayer(Dante_Stats stats, Dante_Skills skills)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(dantePath, FileMode.Create);

        DanteData data = new DanteData(stats, skills);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static DanteData LoadPlayer()
    {
        if (File.Exists(dantePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(dantePath, FileMode.Open);

            DanteData data = formatter.Deserialize(stream) as DanteData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + dantePath);
            return null;
        }
    }

    public static DanteData LoadPlayerNewGame()
    {
        if (File.Exists(danteNewGamePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(danteNewGamePath, FileMode.Open);

            DanteData data = formatter.Deserialize(stream) as DanteData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + danteNewGamePath);
            return null;
        }
    }
    #endregion
    #region Scene
    static string scenePath = Application.persistentDataPath + "/scene.data";

    public static void SaveScene(string sceneName, Vector2 positionOnScene)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(scenePath, FileMode.Create);

        SceneData data = new SceneData(sceneName, positionOnScene);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SceneData LoadScene()
    {
        if (File.Exists(scenePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(scenePath, FileMode.Open);

            SceneData data = formatter.Deserialize(stream) as SceneData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + scenePath);
            return null;
        }
    }
    #endregion
}