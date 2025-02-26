using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void SaveAchieve(AchievementManager aM)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/achivement.lol";
        FileStream stream = new FileStream(path, FileMode.Create);

        AchievData achievData = new AchievData(aM);

        binaryFormatter.Serialize(stream, achievData);
        stream.Close();
    }
    public static AchievData LoadAchieve()
    {
        string path = Application.persistentDataPath + "/achivement.lol";

        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AchievData achieveData = binaryFormatter.Deserialize(stream) as AchievData;
            stream.Close();
            return achieveData;
        }
        else
        {
            Debug.LogError("UhOh: " + path);
            return null;
        }
    }
}
