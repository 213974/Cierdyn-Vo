using System.Collections.Generic;
using UnityEngine;

// An enum to clearly define our character IDs.
public enum CharacterId { Cierdyn, Elara }

// =================================================================================
// DATA DEFINITIONS (Our "Tables" and component data structures)
// =================================================================================

// --- Item & Inventory System ---
[System.Serializable]
public class ItemData
{
    public string itemId;
    public string name;
    public string description;
    public bool isStackable;
}

[System.Serializable]
public class InventorySlotData
{
    public string itemId;
    public int quantity;
}

[System.Serializable]
public class InventoryData
{
    public List<InventorySlotData> slots = new List<InventorySlotData>();
}

// --- Stat System ---
[System.Serializable]
public class StatData
{
    public int health;
    public int maxHealth;
    public int mana;
    public int maxMana;
    public int attackPower;
    public float moveSpeed;
}

// --- Player & Character System ---
// Represents an individual character's unique state in the world.
[System.Serializable]
public class CharacterStateData
{
    public CharacterId id;
    public Vector3 position;
}

// Represents the shared data and progression for the PLAYER.
[System.Serializable]
public class PlayerData
{
    public CharacterId activeCharacterId; // Which character is currently being controlled.
    public StatData stats = new StatData();
    public InventoryData inventory = new InventoryData();
    public List<CharacterStateData> characterStates = new List<CharacterStateData>();
}

// --- Enemy System ---
[System.Serializable]
public class EnemyData
{
    public ulong enemyInstanceId;
    public string enemyTypeId;
    public Vector3 position;
    public StatData stats;
}

// --- World System ---
[System.Serializable]
public class WorldData
{
    public float timeOfDay;
}


// =================================================================================
// THE MASTER DATABASE OBJECT
// This is the single object that gets saved to our JSON file.
// =================================================================================

[System.Serializable]
public class GameData
{
    public WorldData worldData = new WorldData();
    public PlayerData playerData = new PlayerData();
    public List<EnemyData> enemies = new List<EnemyData>();

    // A definition "table" for all possible items in the game.
    public List<ItemData> itemDefinitions = new List<ItemData>();
}