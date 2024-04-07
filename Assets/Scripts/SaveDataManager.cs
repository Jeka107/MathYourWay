using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class JsonHelper
{
    public static List<T> FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper?.Items;
    }

    public static string ToJson<T>(List<T> list)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = list;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(List<T> list, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = list;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public List<T> Items;
    }
}

public class SaveDataManager : MonoBehaviour
{
    private static List<PlayerSavedData> levels= new List<PlayerSavedData>();
    private bool soundEffectStatus;
    public static SaveDataManager Instance;

    private string saveFolderName = "Saved_Data_JSON";
    private string saveFileName = "save_json1.sav";
    private string saveFileNameSettings = "Settings_json1.sav";

    private static bool created = false;
    private string password = "Jeka";
    private bool encrypt = true;

    private void Awake()
    {
        Instance = this;
        soundEffectStatus = LoadSettingsData();

        if (!created)
        {
            if (LoadData() == null)
            {
                levels.Add(new PlayerSavedData(0));
                SaveData();
            }
            else
            {
                levels = LoadData();
            }
            
            SaveSettingsData();

            DontDestroyOnLoad(this.gameObject);
            created = true; 
        }
    }
    #region GamePlaySave
    public void UpdateDataList(int level,int _stars)
    {
        if (levels == null)
            return;

        if (levels[level - 1] != null)
        {
            if (levels[level - 1].stars < _stars)
            {
                levels[level - 1].stars = _stars;

                if (level == levels.Count)//check if we played the last level that opened
                    levels.Add(new PlayerSavedData(0));
            }
        }
        else
        {
            levels.Add(new PlayerSavedData(_stars));
        }

        SaveData();
    }
    public void SaveData()
    {
        string filePath = Application.persistentDataPath + "/" + saveFolderName + "/" + saveFileName;
        
        if (!System.IO.File.Exists(filePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        string dataInJSON = JsonHelper.ToJson(levels, true);
        FileStream fs = new FileStream(filePath, FileMode.Create);

        StreamWriter sw = new StreamWriter(fs);

        if (encrypt)
        {
            dataInJSON = EncryptDecrypt(dataInJSON);
        }

        sw.Write(dataInJSON);
        
        sw.Close();
        fs.Close();
    }
    public List<PlayerSavedData> LoadData()
    {
        string filePath = Application.persistentDataPath + "/" + saveFolderName + "/" + saveFileName;
        string dataToLoad = "";

        if (System.IO.File.Exists(filePath))
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);

            StreamReader sr = new StreamReader(fs);

            dataToLoad = sr.ReadToEnd();

            if (encrypt)
            {
                dataToLoad = EncryptDecrypt(dataToLoad);
            }

            List<PlayerSavedData> loadedData = JsonHelper.FromJson<PlayerSavedData>(dataToLoad);

            sr.Close();
            fs.Close();

            return loadedData;
        }
        else
            return null;
        
    }
    #endregion

    #region SettingsSave

    public void SetSoundStatus(bool _soundEffectStatus)
    {
        soundEffectStatus = _soundEffectStatus;
        SaveSettingsData();
    }

    public void SaveSettingsData()
    {
        PlayerSavedSettings dataToSave = new PlayerSavedSettings(soundEffectStatus);
            
        string filePath = Application.persistentDataPath + "/" + saveFolderName + "/" + saveFileNameSettings;
        //Debug.Log(filePath);
        if (!System.IO.File.Exists(filePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        string dataInJSON = JsonUtility.ToJson(dataToSave, true);

        FileStream fs = new FileStream(filePath, FileMode.Create);

        StreamWriter sw = new StreamWriter(fs);

        if (encrypt)
        {
            dataInJSON = EncryptDecrypt(dataInJSON);
        }

        sw.Write(dataInJSON);

        sw.Close();
        fs.Close();
    }
    public bool LoadSettingsData()
    {
        string filePath = Application.persistentDataPath + "/" + saveFolderName + "/" + saveFileNameSettings;

        string dataToLoad = "";

        if (System.IO.File.Exists(filePath))
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);

            StreamReader sr = new StreamReader(fs);

            dataToLoad = sr.ReadToEnd();

            if (encrypt)
            {
                dataToLoad = EncryptDecrypt(dataToLoad);
            }

            PlayerSavedSettings loadedData =
                JsonUtility.FromJson<PlayerSavedSettings>(dataToLoad);

            soundEffectStatus = loadedData.soundEffectStatus;

            sr.Close();
            fs.Close();

            return soundEffectStatus;
        }
        else
            return true;
        
    }

    #endregion
    private string EncryptDecrypt(string data)
    {
        string newData = "";

        for (int i = 0; i < data.Length; i++)
        {
            newData += (char)(data[i] ^ password[i % password.Length]);
        }

        return newData;
    }
}
