using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;
    [Space]
    [SerializeField] private Respawn_Type waypointType;
    [SerializeField] private Respawn_Type contedWaypoint;
    [SerializeField] private Transform respwanPoint;
    [SerializeField] private bool canBeTriggered = true;

    public Respawn_Type GetWaypointType() => waypointType;
  
    public Vector3 GetPositionAndSetTriggerFalse()
    {
        canBeTriggered = false;
        return respwanPoint == null ? transform.position : respwanPoint.position;
    }

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
      

       GameManager.instance.ChangeScene(transferToScene, contedWaypoint);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true ;
    }
}
