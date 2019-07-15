using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string title;
    public Sprite image;
    public string description;
    public int value = 0;

    public void SetData(Card props = null) {
        title = props.title;
        image = props.image;
        description = props.description;
        value = props.value;
    }

    public void SetData(CardItem props = null)
    {
        title = props.title;
        image = props.image;
        description = props.description;
        value = props.value;
    }

    public CardItem ToItem() {
        CardItem item = new CardItem();

        item.title = title;
        item.description = description;
        item.value = value;
        item.image = image;

        return item;
    }

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }
}
