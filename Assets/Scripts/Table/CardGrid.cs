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
    float spacing = 12;

    List<GameObject> cards = new List<GameObject>(); // todo: change to Card model

    public int maxCards = 6;
    public bool canAddCard;

    private void Awake()
    {
        gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
        gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayoutGroup.constraintCount = 1;
        // todo: calculate width and height of the cell item due to a content dimensions
        Vector2 cardProportions = CardView.proportions;
        Rect rect = GetComponent<RectTransform>().rect;
        int cellCount = Mathf.Max(maxCards, 1);
        float freeWidth = rect.width - ((cellCount) * spacing);
        float cellWidth = freeWidth / cellCount;
        float freeHeight = rect.height - (spacing);
        float cellHeight = Mathf.Min((cellWidth / cardProportions.x) * cardProportions.y, freeHeight); // check that height fit container
        cellWidth = (cellHeight / cardProportions.y) * cardProportions.x; // make sure that proportions are saved

        cellSize = new Vector2(cellWidth, cellHeight);
        cellSpacing = new Vector2(spacing, 0);

        canAddCard = maxCards != 0 && maxCards > cards.Count;
    }

    public void RemoveCard(GameObject card)
    {
        // find and remove
        int index = cards.FindIndex(c => card == c);

        if (index != -1)
        {
            cards.RemoveAt(index);
            canAddCard = maxCards > cards.Count;
        }
    }

    public bool AddCard(GameObject card)
    {
        if (canAddCard)
        {
            cards.Add(card);
            card.transform.SetParent(transform);
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
        gridLayoutGroup.cellSize = cellSize;
        gridLayoutGroup.spacing = cellSpacing;
    }
}
