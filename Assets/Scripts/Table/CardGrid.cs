using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardGrid : MonoBehaviour, IDropHandler
{
    [HideInInspector] public GridLayoutGroup gridLayoutGroup;
    Vector2 cellSize;
    Vector2 cellSpacing;
    Vector2 maxCellSpacing = new Vector2(10, 10);

    public List<Card> cards = new List<Card>(); // todo: change to Card model

    public int maxCards = 6;
    public bool canAddCard;

    private void Awake()
    {
        gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
        gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayoutGroup.constraintCount = 1;

        canAddCard = maxCards != 0 && maxCards > cards.Count;
    }

    void UpdateUI()
    {
        cellSize = UIManager.cardSize;

        if (cellSize == null) { return; }

        Rect rect = GetComponent<RectTransform>().rect;

        float freeWidth = rect.width - (cellSize.x * maxCards);
        float freeHeight = rect.height - cellSize.y; // Only one card row

        float hSpacing = Mathf.Min(freeWidth / 2, maxCellSpacing.x);
        float vSpacing = Mathf.Min(freeHeight / 2, maxCellSpacing.y);

        cellSpacing = new Vector2(hSpacing, vSpacing);

        gridLayoutGroup.cellSize = cellSize;
        gridLayoutGroup.spacing = cellSpacing;
    }

    public void RemoveCard(Card card)
    {
        // find and remove
        int index = cards.FindIndex(c => card == c);

        if (index != -1)
        {
            cards.RemoveAt(index);
            canAddCard = maxCards > cards.Count;
        }
    }

    public bool AddCard(Card card)
    {
        if (canAddCard)
        {
            cards.Add(card);

            canAddCard = maxCards > cards.Count;
            return true;
        }

        return false;
    }

    public void OnDrop(PointerEventData ev)
    {
        GameObject selectedObject = ev.pointerDrag;
        if (selectedObject == null) { return; }

        CardCtrl cardCtrl = selectedObject.GetComponent<CardCtrl>();

        if (cardCtrl != null)
        {
            cardCtrl.possibleCardGrid = this;
        }
    }

    void Start()
    {
        UpdateUI();
    }
}
