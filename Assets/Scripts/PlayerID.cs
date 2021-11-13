using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerID : MonoBehaviour
{
    private string _playerID;

    public bool PlayerHasID()
    {
        string path = Application.persistentDataPath + "/PlayerID.json";
        if (File.Exists(path))
        {
            LoadPlayerID(path);
            return true;
        }
        else
            return false;
    }
    public void CreatePlayerID()
    {
        int idNumber;
        idNumber = Random.Range(0, 999999);
        _playerID = idNumber.ToString();
        SavePlayerID();
    }
    public string GetPlayerID()
    {
        return _playerID;
    }
    [System.Serializable]
    class PlayerIDData
    {
        public string PlayerID;
    }
    private void SavePlayerID()
    {
        PlayerIDData data = new PlayerIDData();
        data.PlayerID = _playerID;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/PlayerID.json", json);
    }
    private void LoadPlayerID(string path)
    {
        string json = File.ReadAllText(path);
        PlayerIDData data = JsonUtility.FromJson<PlayerIDData>(json);

        _playerID = data.PlayerID;
    }
}
