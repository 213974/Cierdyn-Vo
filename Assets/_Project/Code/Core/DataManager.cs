using UnityEngine;
using System.IO;

public class DataManager
{
    private string saveFilePath;
    private GameData gameData;

    // The constructor sets up the path when the manager is created.
    public DataManager()
    {
        // Application.persistentDataPath is the correct, platform-safe
        // location to store user data.
        saveFilePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        gameData = new GameData();
    }

    public GameData GetGameData()
    {
        return gameData;
    }

    public void Load()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            // Overwrite the in-memory gameData with data from the file.
            JsonUtility.FromJsonOverwrite(json, gameData);
            Debug.Log("DataManager: Game data loaded successfully from " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("DataManager: No save file found. Starting with default data.");
            // We don't need to do anything else, as a new `gameData` object
            // was already created in the constructor.
        }
    }

    public void Save()
    {
        // Convert the in-memory gameData object to a JSON string.
        string json = JsonUtility.ToJson(gameData, true); // 'true' for pretty print
        File.WriteAllText(saveFilePath, json);
        Debug.Log("DataManager: Game data saved successfully to " + saveFilePath);
    }
}