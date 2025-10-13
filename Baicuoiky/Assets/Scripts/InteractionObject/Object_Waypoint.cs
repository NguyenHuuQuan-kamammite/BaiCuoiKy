using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;
    [Space]
    public Respawn_Type waypointType;
    [SerializeField]    private Respawn_Type contedWaypoint;
    [SerializeField]   private bool canBeTriggered = true;

    private void OnValidate()
    {
        gameObject.name = "Object Waypoint - " + waypointType.ToString() +  " - " + transferToScene;

        if(waypointType == Respawn_Type.Enter)
            contedWaypoint = Respawn_Type.Exit;
        if(waypointType == Respawn_Type.Exit)
            contedWaypoint = Respawn_Type.Enter;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canBeTriggered == false)
            return;
        SaveManager.instance.SaveGame();

        SceneManager.LoadScene(transferToScene);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true ;
    }
}
