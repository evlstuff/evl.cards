using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject _cardView;
    public Transform _tableLayout;

    static GameManager _this;

    public static GameObject cardView;
    public static Transform tableLayout;

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

        DontDestroyOnLoad(_this);
    }
}
