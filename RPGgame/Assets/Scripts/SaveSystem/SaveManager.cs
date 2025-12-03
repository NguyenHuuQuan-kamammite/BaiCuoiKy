using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> allSaveables;
    [SerializeField] private string fileName = "data.json";
    [SerializeField] private bool encryptData = true;
    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath);
        //  Create the file handler with the save path
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        // Find all saveable objects in the scene
        allSaveables = FindISaveables();


        yield return null;
        LoadGame();
    }
    private void LoadGame()
    {
        gameData = dataHandler.LoadData();

        if (gameData == null)
        {
            Debug.Log("No save data found, creating new save!");
            gameData = new GameData();
            return;
        }

        foreach (var saveable in allSaveables)
            saveable.LoadData(gameData);
    }
    public void SaveGame()
    {
        foreach (var saveable in allSaveables)
            saveable.SaveData(ref gameData);

        dataHandler.SaveData(gameData);
    }
    public GameData GetGameData() => gameData;
    [ContextMenu("*** Delete save data ***")]
    public void DeleteSaveData()
    {
        // Recreate file handler 
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        //Delete the physical save file
        dataHandler.Delete();

        //Create fresh GameData with default values
        gameData = new GameData();

        //Find all saveable objects in the scene
        allSaveables = FindISaveables();

        //Load default data into all game systems
        foreach (var saveable in allSaveables)
            saveable.LoadData(gameData);

        Debug.Log("Save data deleted and reset to defaults!");
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    public bool HasSaveFile()
    {
        return dataHandler.HasSaveFile();
    }
    private List<ISaveable> FindISaveables()
    {
        return
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<ISaveable>()
            .ToList();
    }
}   
