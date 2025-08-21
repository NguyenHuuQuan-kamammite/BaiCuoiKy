using System;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class UI_TreeConnectConnectionDetail
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType direction;
    [Range(100f, 350f)] public float length;
    [Range(-25f, 25f)] public float rotation;
}
public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform myRect => GetComponent<RectTransform>();
    [SerializeField] private UI_TreeConnectConnectionDetail[] connectionDetail;
    [SerializeField] private UI_TreeConnection[] connections;
    private Image connectionImage;
    private Color originalColor;
    private void Awake()
    {
        if(connectionImage != null)
        {
            originalColor = connectionImage.color;
        }
    }

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
    public void UpdateConnections()
    {
        for (int i = 0; i < connectionDetail.Length; i++)
        {
            var connection = connections[i];
            var detail = connectionDetail[i];
            Vector2 targetPosition = connection.GetConnectionPoint(myRect);
            Image connectionImage = connection.GetConnectionImage();
            connection.DirectConnection(detail.direction, detail.length, detail.rotation);

            if (detail.childNode == null)
                continue;

            detail.childNode.SetPosition(targetPosition);
            detail.childNode.SetConnectionImage(connectionImage);
            detail.childNode.transform.SetAsLastSibling();
        }
    }
    public void UpdateAllConnections()
    {
        UpdateConnections();
        foreach (var node in connectionDetail)
        {
            if (node.childNode == null) continue;
            node.childNode.UpdateConnections();
        }
    }
    public void UnlockConnectionImage(bool unlocked)
    {
        if (connectionImage == null)
            return;

        connectionImage.color = unlocked ? Color.white : originalColor;

    }

    public void SetConnectionImage(Image image) => connectionImage = image;
   
   
    public void SetPosition(Vector2 position) => myRect.anchoredPosition = position;

}
