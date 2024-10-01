using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int totalBiscuits;
    public int biscuitsPerSecond;
    public Dictionary<string, int> upgradeLevels; 
}
public class GameSaveManager : MonoBehaviour
{
    private const string SaveKey = "ClickerGameSave";
    public void SaveGame(GameData gameData)
    {
        string jsonData = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString(SaveKey, jsonData);
        PlayerPrefs.Save();
        Debug.Log("Game Saved!"); 
    }
    public GameData LoadGame()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string jsonData = PlayerPrefs.GetString("GameData"); 
            GameData gameData = JsonUtility.FromJson<GameData>(jsonData);
            Debug.Log("Game Loaded!");
            return gameData;
        }
        else
        {
            Debug.Log("No saved game data found"); 
            return null;
        }
    }
    public void ResetGame()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save(); 
        Debug.Log("Game Data Reset!"); 
    }
}
