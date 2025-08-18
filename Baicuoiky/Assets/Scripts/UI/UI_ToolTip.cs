
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private Vector2 offset = new Vector2(300, 20);
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public virtual void ShowToolTip(bool show, RectTransform targetRect)
    {
        if (show == false)
        {
            rect.position = new Vector2(9999, 9999);
            return;
        }
        UpdatePosition(targetRect);
    }
    private void UpdatePosition(RectTransform targetRect)
    {
        float screenTop = Screen.height;
        float screenBottom = 0;
        float screenCenterX = Screen.width / 2f;
        Vector2 targetPosition = targetRect.position;
        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - offset.x : targetPosition.x + offset.x;

        float toolTipHalf = rect.sizeDelta.y / 2f;
        float topY = targetPosition.y + toolTipHalf;
        float bottomY = targetPosition.y - toolTipHalf;

        if (topY > screenTop)
        {
            targetPosition.y = screenTop - toolTipHalf - offset.y;
        }
        else if (bottomY < screenBottom)
        {
            targetPosition.y = screenBottom + toolTipHalf + offset.y;
        }

        rect.position = targetPosition;
    }

}
