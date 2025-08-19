using UnityEngine;

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectLength;
    [SerializeField] private RectTransform childNodeConnectionPoint;
    public void DirectConnection(NodeDirectionType direction, float length)
    {
        bool shouldBeActive = direction != NodeDirectionType.None;
        float finalLength = shouldBeActive ? length : 0f;
        float angle = GetDirectionAngle(direction);

        rotationPoint.localRotation = Quaternion.Euler(0, 0, angle);
        connectLength.sizeDelta = new Vector2(finalLength, connectLength.sizeDelta.y);
    }
public Vector2 GetConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            rect.parent as RectTransform,
           childNodeConnectionPoint.position,
            null, 
            out Vector2 localPosition
        );
        return localPosition;
    }


    private float GetDirectionAngle(NodeDirectionType type)
    {
        switch (type)
        {
            case NodeDirectionType.Up: return 90f;
            case NodeDirectionType.UpRight: return 45f;
            case NodeDirectionType.UpLeft: return 135f;
            case NodeDirectionType.Right: return 0f;
            case NodeDirectionType.Left: return 180f;
            case NodeDirectionType.DownRight: return -45f;
            case NodeDirectionType.Down: return -90f;
            case NodeDirectionType.DownLeft: return -135f;
            default: return 0f;

        }
    }
}
public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Down,
    DownLeft,
    DownRight,
    Left,
    Right
}
