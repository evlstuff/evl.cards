using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorEnums;

public class CardEditorWindow : EditorWindow
{
    public Card card;
    bool isNew; // todo: support setting saved card and update values

    [MenuItem ("Window/Card Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(CardEditorWindow));
    }

    void ClearCard()
    {
        card = new Card();
        isNew = true;
    }

    private void OnEnable()
    {
        if (card == null)
        {
            ClearCard();
        }
    }

    void StartNewLine()
    {
        GUILayout.BeginHorizontal();
    }

    void CloseLine()
    {
        GUILayout.EndHorizontal();
    }

    string DrawStringField(string label, string value, bool newLine = false)
    {
        if (newLine) { StartNewLine(); }
        GUILayout.Label(label);
        string res = GUILayout.TextField(value);
        if (newLine) { CloseLine(); }

        return res;
    }

    int DrawIntField(string label, int value, bool newLine = false)
    {
        if(newLine) { StartNewLine(); }
        GUILayout.Label(label);
        int res = EditorGUILayout.IntField(value);
        if (newLine) { CloseLine(); }

        return res;
    }

    bool DrawButton(string label, bool newLine = false)
    {
        if (newLine) { StartNewLine(); }
        bool res =  GUILayout.Button(label);
        if (newLine) { CloseLine(); }

        return res;
    }

    private void OnGUI()
    {
        // card = EditorGUILayout.ObjectField(card, typeof(Card), true);

        card.name = DrawStringField("Name", card.name, true);
        card.image =  DrawStringField("Image", card.image, true);
        card.description = DrawStringField("Description", card.description, true);
        card.value = DrawIntField("Value", card.value, true);

        StartNewLine();
        if (DrawButton(isNew ? "Add Card" : "Update Card"))
        {
            Debug.Log((isNew ? "Add Card" : "Update Card") + " => " + card.name);
        }
        if (DrawButton("Clear Card"))
        {
            ClearCard();
        }
        CloseLine();
    }
}
