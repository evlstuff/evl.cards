using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardCtrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    CanvasGroup cGroup;

    [HideInInspector] public Card card;
    [HideInInspector] public CardGrid parentCardGrid;
    [HideInInspector] public int siblingIndex;
    [HideInInspector] public CardGrid possibleCardGrid;

    private void Start()
    {
        card = GetComponent<Card>();
        cGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData ev)
    {
        siblingIndex = transform.GetSiblingIndex();
        parentCardGrid = transform.parent.GetComponent<CardGrid>();
        transform.SetParent(GameManager.tableLayout); // put the card to layout

        if (cGroup != null)
        {
            cGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData ev)
    {
        transform.position = ev.position;
    }

    public void OnEndDrag(PointerEventData ev)
    {

        CardGrid targetCardGrid = possibleCardGrid == null || !possibleCardGrid.canAddCard ? parentCardGrid : possibleCardGrid;

        if (targetCardGrid != null)
        {
            transform.SetParent(targetCardGrid.transform);
        }

        if (targetCardGrid != parentCardGrid)
        {
            parentCardGrid.RemoveCard(card);
            targetCardGrid.AddCard(card);
            siblingIndex = transform.GetSiblingIndex();
        }
        else
        {
            transform.SetSiblingIndex(siblingIndex);
        }

        if (cGroup != null)
        {
            cGroup.blocksRaycasts = true;
        }

        parentCardGrid = null;
        possibleCardGrid = null;
    }
}

