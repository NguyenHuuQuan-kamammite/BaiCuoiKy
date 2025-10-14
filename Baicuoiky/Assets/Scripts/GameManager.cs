using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
public class GameManager: MonoBehaviour
{
    public static GameManager instance;

    private Vector3 lastPlayerPosition;
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
    public void SetLastPlayerPosition(Vector3 position) => lastPlayerPosition = position;
    public void RestartScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, Respawn_Type.None);
    }

    public void ChangeScene(string sceneName, Respawn_Type respwanType)
    {
       
        StartCoroutine(ChangeSceneCo(sceneName, respwanType));
    }



    private IEnumerator ChangeSceneCo(string sceneName, Respawn_Type respawnType)
    {
       
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(.2f);
        Vector3 position = GetNewPlayerPosition(respawnType);
        if (position != Vector3.zero)
            Player.instance.TeleportPlayer(position);
    }

    private Vector3 GetNewPlayerPosition(Respawn_Type type)
    {
        if (type == Respawn_Type.None)
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

            var selectedPositions = unlockedCheckpoints.Concat(enterWaypoints).ToList();// combine two lists into one

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

}
