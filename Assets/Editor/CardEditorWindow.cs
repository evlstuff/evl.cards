using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;
using EditorEnums;

public class CardEditorWindow : EditorWindow
{
    bool isNew; // todo: support setting saved card and update values
    string output;

    CardItem SelectedCard;

    string Name = "New Card";
    string Image;
    string Description = "";
    int Value = 0;

    [MenuItem ("Window/Card Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(CardEditorWindow));
    }

    private void OnEnable()
    {
       
    }

    void ClearCard()
    {
        SetData();
    }

    void SetData(CardItem card = null) {
        Name = card == null ? "" : card.name;
        Image = card == null ? null : card.image;
        Description = card == null ? null : card.description;
        Value = card == null ? 0 : card.value;

        isNew = card == null;
        SelectedCard = isNew ? null : SelectedCard;
        output = "";
    }

    bool ValidateCard() {
        if (Name == null || Name == "") {
            output = "Name is required";
            return false;
        }

        if (Image == null || Image == "")
        {
            output = "Image is required";
            return false;
        }

        return true;
    }

    CardItem GetCardData() {
        CardItem asset = ScriptableObject.CreateInstance<CardItem>();
        asset.name = Name;
        asset.image = Image;
        asset.description = Description;
        asset.value = Value;

        return asset;
    }

    void SaveCard() {
        bool isValid = ValidateCard();

        if (!isValid) { return; }

        CardItem asset = GetCardData();
        string path = "Assets/Data/" + asset.name + ".asset";
        bool exists = File.Exists(path);

        if (exists) {
            output = "Card with this name already exist";
            return;
        }

        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();

        ClearCard();
    }
    
    void UpdateCard() {
        bool isValid = ValidateCard();

        if (!isValid) { return; }

        CardItem asset = GetCardData();
        AssetDatabase.CreateAsset(asset, "Assets/Data/" + asset.name + ".asset");
        AssetDatabase.SaveAssets();

        ClearCard();
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

    bool DrawButton(string label, bool enabled = true, bool newLine = false)
    {
        if (newLine) { StartNewLine(); }
        GUI.enabled = enabled;
        bool res =  GUILayout.Button(label);
        GUI.enabled = true;
        if (newLine) { CloseLine(); }

        return res;
    }

    private void OnGUI()
    {
        GUILayout.Space(20);
        SelectedCard = (CardItem)EditorGUILayout.ObjectField("Card", SelectedCard, typeof(CardItem), true);
        if (DrawButton("Confirm")) {
            SetData(SelectedCard);
        }
        GUILayout.Space(10);

        Name = DrawStringField("Name", Name, true);
        Image =  DrawStringField("Image", Image, true);
        Description = DrawStringField("Description", Description, true);
        Value = DrawIntField("Value", Value, true);

        GUILayout.Space(10);
        StartNewLine();
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.normal.textColor = Color.red;
        GUILayout.Label(output, style);
        CloseLine();

        StartNewLine();
        if (DrawButton("Update Card", !isNew))
        {
            UpdateCard();
        }
        if (DrawButton("Save Card"))
        {
            SaveCard();
        }
        if (DrawButton("Clear Card"))
        {
            ClearCard();
        }
        CloseLine();
    }
}
