using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    Card card;
    public static Vector2 proportions = new Vector2(2, 3);
    public Image image;
    public Text value;
    public Text title;
    public Text description;

    private void Awake()
    {
        card = GetComponent<Card>();
    }

    Color RandomColor() {
        Color color = Random.ColorHSV();
        float minColorValue = .35f;

        if (color.r < minColorValue || color.g < minColorValue || color.b < minColorValue) {
            color = RandomColor();
        }

        return color;
    }

    void Start()
    {
        if (image != null)
        {
            image.color = RandomColor();
        }

        if (card == null) return;

        if (!title.IsNullOrEmpty() && !card.title.IsNullOrEmpty()) {
            title.text = card.title; 
        }
        if (!description.IsNullOrEmpty() && !card.description.IsNullOrEmpty())
        {
            description.text = card.description;
        }
        if (!value.IsNullOrEmpty() && !card.value.IsNullOrEmpty())
        {
            value.text = card.value.ToString();
        }
    }
}
