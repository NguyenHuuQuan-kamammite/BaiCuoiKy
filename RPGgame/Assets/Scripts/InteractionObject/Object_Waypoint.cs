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
    [SerializeField] private bool isActive = true;

    public Respawn_Type GetWaypointType() => waypointType;
  
    public Vector3 GetPositionAndSetTriggerFalse()
    {
        canBeTriggered = false;
        return respwanPoint == null ? transform.position : respwanPoint.position;
    }

    public void ActivateWaypoint()
    {
        isActive = true;
        Debug.Log("Waypoint activated!");
    }

    // Public method to deactivate the waypoint (optional)
    public void DeactivateWaypoint()
    {
        isActive = false;
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
        if (isActive == false || canBeTriggered == false)
        {
            Debug.Log($"Waypoint trigger blocked. isActive: {isActive}, canBeTriggered: {canBeTriggered}");
            return;
        }

        Debug.Log($"Waypoint triggered! Changing scene to {transferToScene}");
        GameManager.instance.ChangeScene(transferToScene, contedWaypoint);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true ;
    }
}
