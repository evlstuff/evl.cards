using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
[System.Serializable]
public class CardItem : ScriptableObject
{
    public string title;
    public Sprite image;
    public string description;
    public int value;

    public Card ToClass()
    {
        Card card = new Card();

        card.title = title;
        card.description = description;
        card.value = value;
        card.image = image;

        return card;
    }

    public CardItem ToAsset() {
        CardItem asset = ScriptableObject.CreateInstance<CardItem>();

        string pattern = @"\s";
        string name = Regex.Replace(title, pattern, "_").ToLower();

        asset.name = name;
        asset.title = title;
        asset.description = description;
        asset.value = value;

        return asset;
    }
}
