using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
[System.Serializable]
public class CardItem : ScriptableObject
{
    public string title;
    public Sprite image;
    public string description;
    public int value;
}
