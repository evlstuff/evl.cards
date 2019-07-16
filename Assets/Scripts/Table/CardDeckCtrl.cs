using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDeckCtrl : MonoBehaviour, IPointerClickHandler
{
    CardGrid cardGrid;
    EventTrigger eventTrigger;
    CardGrid tableGrid;

    public List<CardItem> cards;
    bool hasCards = false;

    private void Awake()
    {
        cardGrid = GetComponent<CardGrid>();
    }

    private void Start()
    {
        tableGrid = UIManager.GetTableGrid();
        Instantiate(UIManager.GetDeckPrefab(), transform);
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

        if (tableGrid != null && tableGrid.canAddCard)
        {
            var cardObject = Instantiate(UIManager.GetCardPrefab(), tableGrid.transform);
            Card card = cardObject.GetComponent<Card>();
            CardItem cardItem = cards[0];

            card.SetData(cardItem);

            tableGrid.AddCard(card);

            cards.RemoveAt(0);
            hasCards = cards.Count > 0;
        }
    }
}
