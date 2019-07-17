using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSize : MonoBehaviour
{
    #region Public Props
    public ESize size;
    public Vector2 scale;
    #endregion
    #region Private Props
    RectTransform rTransform;
    RectTransform parentRTransform;
    #endregion
    #region Debug Props
    public bool showDebug;
    public Vector2 parentSize;
    public Vector2 offsetSize;
    public Color debugColor = Color.cyan;
    #endregion

    void SetSize(float width, float height)
    {
        rTransform.sizeDelta = offsetSize = new Vector2(width, height);
        parentSize = new Vector2(parentRTransform.rect.width, parentRTransform.rect.height);
    }

    void ScaleToParent()
    {
        Rect parent = parentRTransform.rect;
        float width = parent.width * scale.x;
        float height = parent.height * scale.y;
        SetSize(width, height);
    }

    void FitParentSize()
    {
        Rect parent = parentRTransform.rect;
        SetSize(parent.width, parent.height);
    }

    void UpdateSize()
    {
        if (parentRTransform == null) { return; }

        switch(size)
        {
            case ESize.Contain:
                {
                    ScaleToParent();
                    break;
                }
            case ESize.Cover:
                {
                    FitParentSize();
                    break;
                }
        }
    }

    void Init()
    {
        if (rTransform == null)
        {
            rTransform = GetComponent<RectTransform>();
        }

        if (parentRTransform != null) { return; }
        parentRTransform = transform.parent.GetComponent<RectTransform>();
    }

    Vector3 GetCenterPosition()
    {
        float x = rTransform.localPosition.x;
        float y = rTransform.localPosition.y;

        float offsetX = x > 0 ? 1 : (x < 0 ? -1 : 0);
        float offsetY = y > 0 ? 1 : (y < 0 ? -1 : 0);

        float xFactor = rTransform.pivot.x == 0.5 ? 1 : 0.5f;
        float yFactor = rTransform.pivot.y == 0.5 ? 1 : 0.5f;

        Vector3 offset = rTransform.sizeDelta * (new Vector2(offsetX, offsetY)) * (new Vector2(xFactor, yFactor));
        return parentRTransform.position + rTransform.localPosition - offset;
    }

    [ContextMenu("Reset")]
    private void Reset()
    {
        parentRTransform = null;
    }

    [ContextMenu("Update")]
    private void Update()
    {
        Init();
        UpdateSize(); // TODO: optimize the amount of unnecessary calls
    }

    private void OnDrawGizmos()
    {
        if (!showDebug) { return; }

        Update();
        Vector3 center = GetCenterPosition();
        Color color = debugColor;

        if (color.a > 0.3f) {
            color.a = 0.3f;
        }

        Gizmos.color = color;
        Gizmos.DrawCube(center, rTransform.sizeDelta);
    }
}
