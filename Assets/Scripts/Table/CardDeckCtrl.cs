using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDeckCtrl : MonoBehaviour, IPointerClickHandler
{
    EventTrigger eventTrigger;
    public CardGrid activeCardsGrid;

    public void OnPointerClick(PointerEventData ev)
    {
        if (GameManager.cardView != null && activeCardsGrid != null && activeCardsGrid.canAddCard)
        {
            GameObject card = Instantiate(GameManager.cardView, GameManager.tableLayout);
            activeCardsGrid.AddCard(card);
        }
    }
}
