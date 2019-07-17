using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardGrid : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public GridLayoutGroup gridLayoutGroup;
    Vector2 cellSize;
    Vector2 cellSpacing;
    Vector2 maxCellSpacing = new Vector2(10, 10);
    Image image;

    public List<Card> cards = new List<Card>(); // todo: change to Card model

    public int maxCards = 6;
    public bool canAddCard;
    public bool isActive;
    public bool isHover;

    private void Awake()
    {
        image = GetComponent<Image>();

        gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
        gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayoutGroup.constraintCount = 1;

        canAddCard = maxCards != 0 && maxCards > cards.Count;
    }

    void UpdateGridSize()
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

    public void CheckStatus()
    {
        isActive = isHover && GameManager.isCardGrabbed && canAddCard;
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

    public void OnPointerEnter(PointerEventData ev)
    {
        isHover = true;
    }

    public void OnPointerExit(PointerEventData ev)
    {
        isHover = false;
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

    void UpdateColor()
    {
        if (image == null) { return; }

        if (isActive)
        {
            image.color = UIManager.GetColor(UIColor.ActivePanel);
            return;
        }

        if (GameManager.isCardGrabbed && canAddCard) {
            image.color = UIManager.GetColor(UIColor.HighlightPanel);
            return;
        }

        image.color = UIManager.GetColor(UIColor.DefaultPanel);
    }

    void Start()
    {
        UpdateGridSize();
    }

    private void Update()
    {
        CheckStatus();
        UpdateColor();
    }
}
