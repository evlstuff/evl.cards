using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ElementType
{
    Container, Spacing,
}

public enum ElementDirection
{
    Horizontal, Vertical,
}

public class UIElement : MonoBehaviour
{
    public float scale;
    public ElementType type;
    public ElementDirection direction;

    RectTransform rectT;

    private void Init()
    {
        rectT = GetComponent<RectTransform>();
    }

    private void Awake()
    {
        Init();
    }

    void SetSize(Vector2 size)
    {
        Vector2 defaultSize = rectT.sizeDelta;

        if (size.x == -1 || size.y == -1) {
            RectTransform parent = transform.parent.GetComponent<RectTransform>();
            defaultSize = parent != null ? parent.sizeDelta : defaultSize;
        }

        float width = size.x >= 0 ? size.x : defaultSize.x;
        float height = size.y >= 0 ? size.y : defaultSize.y;

        rectT.sizeDelta = new Vector2(width, height);
    }

    public void SetUnitDimensions(Vector2 unit)
    {
        if (rectT == null)
        {
            Init();
        }

        switch(direction)
        {
            case ElementDirection.Horizontal:
                {
                    SetSize(new Vector2(unit.x * scale, -1));
                    break;
                }
            case ElementDirection.Vertical:
                {
                    SetSize(new Vector2(-1, unit.y * scale));
                    break;
                }
        }
    }
}
