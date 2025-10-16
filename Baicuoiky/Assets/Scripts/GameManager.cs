using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
public class GameManager: MonoBehaviour, ISaveable
{
    public static GameManager instance;

    private Vector3 lastPlayerPosition;
    private string lastScenePlayed;
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
    //public void SetLastPlayerPosition(Vector3 position) => lastPlayerPosition = position;
    public void ContinuePlay()
    {
        ChangeScene(lastScenePlayed, Respawn_Type.NoneSpecific);
    }
    public void RestartScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, Respawn_Type.NoneSpecific);
    }

    public void ChangeScene(string sceneName, Respawn_Type respwanType)
    {
        SaveManager.instance.SaveGame();

        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCo(sceneName, respwanType));
    }



    private IEnumerator ChangeSceneCo(string sceneName, Respawn_Type respawnType)
    {
       
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(1f);


        Player player = Player.instance;

        if (player == null)
            yield break;

        Vector3 position = GetNewPlayerPosition(respawnType);

        if (position != Vector3.zero) 
            player.TeleportPlayer(position);
        
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

       
    }

    public void SaveData(ref GameData data)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MainMenu")
            return;

        data.lastPlayerPosition = Player.instance.transform.position;
        data.lastScenePlayed = currentScene;
    
    }
}
