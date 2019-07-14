using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public string title = "New Card";
    public string image;
    public string description = "";
    public int value = 0;
}

[System.Serializable]
public class Deck
{
    public Card[] cards;
}

[System.Serializable]
public class DeckItem : ScriptableObject
{
    public CardItem[] cards;
}
