using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDeckCtrl : MonoBehaviour, IPointerClickHandler
{
    EventTrigger eventTrigger;
    CardGrid activeCardsGrid;
    public List<CardItem> cards;
    bool hasCards = false;

    private void Start()
    {
        activeCardsGrid = GameManager.activeCardsGrid;
    }

    void GetCards()
    {
        CardItem[] deckCards = GameManager.deck.cards;

        cards.AddRange(deckCards);

        hasCards = cards.Count > 0;
    }

    public void OnPointerClick(PointerEventData ev)
    {
        if (cards == null || cards.Count == 0) {
            //TODO: do it async at start
            GetCards();
        }

        if (activeCardsGrid != null && activeCardsGrid.canAddCard)
        {
            var cardObject = Instantiate(GameManager.cardView, activeCardsGrid.transform);
            Card card = cardObject.GetComponent<Card>();
            CardItem cardItem = cards[0];

            card.SetData(cardItem);
            
            activeCardsGrid.AddCard(card);

            cards.RemoveAt(0);
            hasCards = cards.Count > 0;
        }
    }
}
