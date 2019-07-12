using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Card : Object
{
    public new string name = "New Card";
    public string image;
    public string description = "";
    public int value = 0;
}

public class CardItem : ScriptableObject
{
    public new string name;
    public string image;
    public string description;
    public int value;
}

public class CardItemsBundle : ScriptableObject
{
    public CardItem[] cards;
}
