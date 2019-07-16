using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _this;
    public static DeckItem deck;

    private void Awake()
    {
        if (_this != null && this != _this)
        {
            Destroy(gameObject);
            return;
        }

        _this = this;

        DontDestroyOnLoad(_this);
    }

    private void Start()
    {
        LoadDeck();
    }

    void LoadDeck() {
        // TODO: get bundle with endpoint
        string bundlePath = "Assets/Data/StandaloneWindows/default";
        deck = Editor.LoadAssetBundle(bundlePath);
        Debug.Log("Loaded deck bundle with " + deck.cards.Length + " cards ");
    }
}
