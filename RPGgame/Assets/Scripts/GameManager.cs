using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
public class GameManager: MonoBehaviour, ISaveable
{
    public static GameManager instance;

    private Vector3 lastPlayerPosition;
    private string lastScenePlayed;
    private bool dataLoaded;
   
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

       
    }
    
    public void ContinuePlay()
    {
        if (string.IsNullOrEmpty(lastScenePlayed))
        {
            Debug.LogWarning("No last scene found! Starting from Level_0");
            lastScenePlayed = "Level_0";
        }

        ChangeScene(lastScenePlayed, Respawn_Type.NoneSpecific);
    }
    public void RestartScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, Respawn_Type.NoneSpecific);
    }

    public void ChangeScene(string sceneName, Respawn_Type respwanType)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Cannot change to empty scene name!");
            return;
        }
        SaveManager.instance.SaveGame();

        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCo(sceneName, respwanType));
    }


    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Cannot load empty scene name!");
            return;
        }
        Audio_Manager.instance.StopBGM();
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene != "MainMenu" && currentScene != "EndScene")
        {
            SaveManager.instance.SaveGame();
        }

        StartCoroutine(LoadSceneCo(sceneName));
    }

   
   
    // NEW METHOD: Specifically for returning to main menu
    public void ReturnToMainMenu()
    {
        LoadScene("MainMenu");
    }

    private IEnumerator LoadSceneCo(string sceneName)
    {
        UI_FadeScreen fadeScreen = FindFadeScreenUI();
        fadeScreen.DoFadeOut(); // transparent > black
        yield return fadeScreen.fadeEffectCo;

        SceneManager.LoadScene(sceneName);

        // Reset timescale when returning to main menu
        Time.timeScale = 1;

        fadeScreen = FindFadeScreenUI();
        fadeScreen.DoFadeIn(); // black > transparent
    }
    private IEnumerator ChangeSceneCo(string sceneName, Respawn_Type respawnType)
    {

        UI_FadeScreen fadeScreen = FindFadeScreenUI();

        fadeScreen.DoFadeOut(); // transperent > black
        yield return fadeScreen.fadeEffectCo;

        SceneManager.LoadScene(sceneName);

        dataLoaded = false; // data loaded becomes true when load game from save manager
        yield return null;

        while (dataLoaded == false)
        {
            yield return null;
        }

        fadeScreen = FindFadeScreenUI();
        fadeScreen.DoFadeIn(); // black > transperent

        Player player = Player.instance;

        if (player == null)
            yield break;

        Vector3 position = GetNewPlayerPosition(respawnType);

        if (position != Vector3.zero) 
            player.TeleportPlayer(position);
        
    }



    private UI_FadeScreen FindFadeScreenUI()
    {
        if (UI.instance != null)
            return UI.instance.fadeScreenUI;
        else
            return FindFirstObjectByType<UI_FadeScreen>();
    }



    private Vector3 GetNewPlayerPosition(Respawn_Type type)
    {
        if(type == Respawn_Type.Portal)
        {
            Object_Portal portal = Object_Portal.instnace;

            Vector3 position = portal.GetPosition();

            portal.SetTrigger(false);
            portal.DisableIfNeeded();

            return position;
        }


        if (type == Respawn_Type.NoneSpecific)
        {
            var data = SaveManager.instance.GetGameData();
            var checkpoints = FindObjectsByType<Object_CheckPoint>(FindObjectsSortMode.None);
            var unlockedCheckpoints = checkpoints
                .Where(cp => data.unlockedCheckpoints.TryGetValue(cp.GetCheckpointId(), out bool unlocked) && unlocked)
                .Select(cp => cp.GetPosition())
                .ToList();

            var enterWaypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None)
                .Where(wp => wp.GetWaypointType() == Respawn_Type.Enter)
                .Select(wp => wp.GetPositionAndSetTriggerFalse())
                .ToList();

            var selectedPositions = unlockedCheckpoints.Concat(enterWaypoints).ToList(); // combine two lists into one

            if (selectedPositions.Count == 0)
                return Vector3.zero;

            return selectedPositions.
                OrderBy(position => Vector3.Distance(position, lastPlayerPosition)) // arrange form lowest to highest by comparing distance
                .First();
        }

        return GetWaypointPosition(type);
    }



    private Vector3 GetWaypointPosition(Respawn_Type type)
    {
        var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

        foreach (var point in waypoints)
        {
            if (point.GetWaypointType() == type)
            {
                return point.GetPositionAndSetTriggerFalse();
            }
        }

        return Vector3.zero;
    }



    public void LoadData(GameData data)
    {
        lastScenePlayed = data.lastScenePlayed;
        lastPlayerPosition = data.lastPlayerPosition;

        if (string.IsNullOrEmpty(lastScenePlayed))
            lastScenePlayed = "Level_0";

        dataLoaded = true;
    }




    public void SaveData(ref GameData data)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MainMenu" || currentScene == "EndScene")
            return;


        data.lastPlayerPosition = Player.instance.transform.position;
        data.lastScenePlayed = currentScene;
        dataLoaded = false;
    
    }
}
