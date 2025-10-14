using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{
    public static GameManager instance;
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
    public void ChangeScene(string sceneName, Respawn_Type respwanType)
    {
       
        StartCoroutine(ChangeSceneCo(sceneName, respwanType));
    }



    private IEnumerator ChangeSceneCo(string sceneName, Respawn_Type respawnType)
    {
       
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(.2f);
        Vector3 position = GetWaypointPosition(respawnType);
        if (position != Vector3.zero)
            Player.instance.TeleportPlayer(position);
    }
    private Vector3 GetWaypointPosition(Respawn_Type type)
    {
        var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

        foreach (var point in waypoints)
        {
            if (point.GetWaypointType() == type)
            {
                point.SetCanBeTrigger(false);
                return point.GetPositionAndSetTriggerFalse();
            }
        }

        return Vector3.zero;
    }

}
