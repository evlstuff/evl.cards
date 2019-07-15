using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject _cardView;
    public GameObject _deckPrefab;
    public Transform _tableLayout;
    public CardGrid _activeCardsGrid;

    static GameManager _this;

    public static GameObject deckPrefab;
    public static GameObject cardView;

    public static Transform tableLayout;
    public static CardGrid activeCardsGrid;

    public static DeckItem deck;

    private void Awake()
    {
        if (_this != null && this != _this)
        {
            Destroy(gameObject);
            return;
        }

        _this = this;
        cardView = _cardView;
        tableLayout = _tableLayout;
        deckPrefab = _deckPrefab;
        activeCardsGrid = _activeCardsGrid;

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
