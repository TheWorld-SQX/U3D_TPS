using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : Singleton<SaveManager>
{
    [SerializeField] private GameObject playerGO;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }
    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStates.characterData, GameManager.Instance.playerStates.characterData.name);
    }
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStates.characterData, GameManager.Instance.playerStates.characterData.name);
    }
    public void Save(Object data,string key)
    {
        var jsonDate = JsonUtility.ToJson(data,true);
        PlayerPrefs.SetString(key, jsonDate);
        PlayerPrefs.Save();
    }
    public void Load(Object data,string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }

    public void SavePlayerPosition()
    {
        Save1(playerGO.transform.position, "SP");
    }
    public void LoadPlayerPosition()
    {
        Load1(playerGO.transform.position,"LP");
    }

    public void Save1(Vector3 data, string key)
    {
        var jsonDate = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonDate);
        PlayerPrefs.Save();
    }
    public void Load1(Vector3 data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }

}
