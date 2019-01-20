using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem {
    
    public static void SaveHighScores(HighScore[] highScores)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/highscores.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, highScores);
        stream.Close();
    }

    public static HighScore[] LoadHighScores()
    {
        string path = Application.persistentDataPath + "/highscores.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            HighScore[] highScores = formatter.Deserialize(stream) as HighScore[];
            stream.Close();

            return highScores;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
