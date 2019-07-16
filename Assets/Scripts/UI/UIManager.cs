using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager _this;

    [ContextMenuItem("Update", "UpdateEditLayout")]
    public Transform targetLayout;

    public Vector2 cardScale;
    public Vector2 cardProportions;
    [ContextMenuItem("Update", "UpdateCardSize")]
    public static Vector2 cardSize;

    Vector2 GetElementScale(UIElement elem)
    {
        Vector2 scale = Vector2.zero;

        if (elem == null) { return scale; }

        switch(elem.direction)
        {
            case ElementDirection.Horizontal:
                {
                    scale.x = elem.scale;
                    break;
                }
            case ElementDirection.Vertical:
                {
                    scale.y = elem.scale;
                    break;
                }
        }
        return scale;
    }

    void NormalizeLayout(Transform layout)
    {
        if (layout == null) { return; }

        Rect rect = layout.GetComponent<RectTransform>().rect;
        int count = layout.transform.childCount;
        int totalHorizontal = 0;
        int totalVertical = 0;
        List<UIElement> elements = new List<UIElement>();

        for(int i = 0; i < count; i++)
        {
            Transform target = layout.transform.GetChild(i);
            UIElement elem = target.GetComponent<UIElement>();

            if (elem != null) {
                elements.Add(elem);
                Vector2 elemScale = GetElementScale(elem);
                totalHorizontal += (int)elemScale.x;
                totalVertical += (int)elemScale.y;
            } else
            {
                target.localScale = Vector3.zero;
            }
        }

        Vector2 unitDimensions = new Vector2(rect.width / totalHorizontal, rect.height / totalVertical);

        foreach(UIElement elem in elements)
        {
            elem.SetUnitDimensions(unitDimensions);

            NormalizeLayout(elem.transform);
        }
    }

    void UpdateEditLayout()
    {
        NormalizeLayout(targetLayout);
    }

    void UpdateCardSize()
    {
        if (targetLayout == null) {
            Debug.LogError("No Layout selected");
            return;
        }

        if (cardScale == null)
        {
            Debug.LogError("No Card Scale selected");
            return;
        }

        if (cardProportions == null)
        {
            Debug.LogError("No Card Proportions selected");
            return;
        }

        Rect rect = targetLayout.GetComponent<RectTransform>().rect;
        float scaleWidth = rect.width * cardScale.x;
        float scaleHeight = rect.height * cardScale.y;

        float expectedWidth = scaleWidth;

        float height = Mathf.Min((scaleWidth / cardProportions.x) * cardProportions.y, scaleHeight);
        float width = (height / cardProportions.y) * cardProportions.x;

        cardSize = new Vector2(Mathf.FloorToInt(width), Mathf.FloorToInt(height));
    }

    private void Awake()
    {
        if (_this != null && this != _this)
        {
            Destroy(gameObject);
            return;
        }

        _this = this;

        NormalizeLayout(targetLayout);
        UpdateCardSize();

        DontDestroyOnLoad(_this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
