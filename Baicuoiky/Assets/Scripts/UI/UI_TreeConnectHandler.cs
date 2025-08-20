
using UnityEngine;


[System.Serializable]
public class UI_TreeConnectConnectionDetail
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType direction; 
    [Range(100f, 350f)]  public float length;
}
public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform myRect => GetComponent<RectTransform>();
    [SerializeField] private UI_TreeConnectConnectionDetail[] connectionDetail;
    [SerializeField] private UI_TreeConnection[] connections;

    private void OnValidate()
    {
        if (connectionDetail.Length <= 0)
            return;
        if (connectionDetail.Length != connections.Length)
            {
                Debug.LogError("ConnectionDetail and Connections arrays must have the same length. " + gameObject.name);
                return;
            }
        UpdateConnections();
    }
    private void UpdateConnections()
    {
        for (int i = 0; i < connectionDetail.Length; i++)
        {
            var connection = connections[i];
            var detail = connectionDetail[i];
            Vector2 targetPosition = connection.GetConnectionPoint(myRect);
            connection.DirectConnection(detail.direction, detail.length);
            detail.childNode?.SetPosition(targetPosition);
        }
    }
    public void SetPosition(Vector2 position) => myRect.anchoredPosition = position;

}
