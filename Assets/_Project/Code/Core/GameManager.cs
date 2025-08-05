using UnityEngine;
using System.Linq; // We need this for LINQ queries

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject characterVesselPrefab; // Renamed for clarity

    private DataManager dataManager;

    void Awake()
    {
        dataManager = new DataManager();
        dataManager.Load();

        if (dataManager.GetGameData().playerData.characterStates.Count == 0)
        {
            CreateInitialData();
        }

        SpawnPlayerSquad();
    }

    private void CreateInitialData()
    {
        Debug.Log("Creating initial player and character data.");
        var playerData = dataManager.GetGameData().playerData;

        playerData.activeCharacterId = CharacterId.Cierdyn;
        playerData.stats = new StatData { health = 100, maxHealth = 100, attackPower = 10 };

        playerData.characterStates.Add(new CharacterStateData { id = CharacterId.Cierdyn, position = new Vector3(0, 1, 0) });
        playerData.characterStates.Add(new CharacterStateData { id = CharacterId.Elara, position = new Vector3(2, 1, 0) });

        dataManager.Save();
    }

    private void SpawnPlayerSquad()
    {
        var playerData = dataManager.GetGameData().playerData;

        // Spawn the two character vessels
        CharacterMonoBehaviour cierdyn = null;
        CharacterMonoBehaviour elara = null;

        foreach (var charState in playerData.characterStates)
        {
            GameObject vesselInstance = Instantiate(characterVesselPrefab, charState.position, Quaternion.identity);
            var vesselScript = vesselInstance.GetComponent<CharacterMonoBehaviour>();
            vesselScript.Initialize(charState);

            if (charState.id == CharacterId.Cierdyn) cierdyn = vesselScript;
            if (charState.id == CharacterId.Elara) elara = vesselScript;
        }

        // Hand control over to the PlayerController
        playerController.PossessSquad(cierdyn, elara, playerData.activeCharacterId);
    }
}