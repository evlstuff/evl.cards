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

    void Start()
    {
        if (image != null)
        {
            image.color = Random.ColorHSV();
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
