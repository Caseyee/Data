using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataSaver : MonoBehaviour
{
    public int bestScore;
    public string playerName;

    [System.Serializable]
    public class SaveData
    {
        public int BestScore;
        public string Name;
    }

    public void SaveInfo()
    {
        SaveData data = new SaveData();
        data.BestScore = bestScore;
        data.Name = playerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.BestScore;
            playerName = data.Name;
        }
    }
}
