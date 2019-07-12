using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;
using EditorEnums;
using EditorUtilities;

public class CardEditorWindow : EditorWindow
{
    bool isNew; // todo: support setting saved card and update values
    string output;
    string Path;
    string defaultPath = "Assets/Data/";

    CardItem SelectedCard;

    string Name = "New Card";
    string Image;
    string Description = "";
    int Value = 0;

    [MenuItem ("Window/Card/Card Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(CardEditorWindow), false, "Card Editor");
    }

    private void OnEnable()
    {
        if (Path == null || Path == "") {
            Path = defaultPath;
        }
        ClearCard();
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
        output = "";
    }

    bool Validate() {
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

    void ValidatePath()
    {
        bool isValid = AssetDatabase.IsValidFolder(Path);

        if (!isValid) {
            output = Path + " is not valid";
        }
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
        bool isValid = Validate();

        if (!isValid) { return; }

        CardItem asset = GetCardData();
        string path = Path + asset.name + ".asset";
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
        bool isValid = Validate();

        if (!isValid) { return; }
        if (SelectedCard == null) { return; }

        string path = AssetDatabase.GetAssetPath(SelectedCard);

        CardItem asset = GetCardData();
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();

        ClearCard();
    }

    void DeleteCard()
    {
        if (SelectedCard == null) { return;  }

        string path = AssetDatabase.GetAssetPath(SelectedCard);
        bool exists = File.Exists(path);

        if (!exists) {
            output = "There is no such card file";
            return;
        }

        AssetDatabase.DeleteAsset(path);

        SelectedCard = null;
        ClearCard();
    }

    private void OnGUI()
    {
        GUILayout.Space(20);
        SelectedCard = (CardItem)EditorGUILayout.ObjectField("Card", SelectedCard, typeof(CardItem), true);
        if (GUIWindow.DrawButton("Confirm", SelectedCard != null)) {
            SetData(SelectedCard);
        }
        GUILayout.Space(10);

        Name = GUIWindow.DrawStringField("Name", Name, true);
        Image = GUIWindow.DrawStringField("Image", Image, true);
        Description = GUIWindow.DrawStringField("Description", Description, true);
        Value = GUIWindow.DrawIntField("Value", Value, true);

        GUILayout.Space(10);
        GUIWindow.StartNewLine();
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.normal.textColor = Color.red;
        GUILayout.Label(output, style);
        GUIWindow.CloseLine();

        GUIWindow.StartNewLine();
        GUI.enabled = false;
        Path = GUIWindow.DrawStringField("Path", Path);
        GUI.enabled = true;
        if (GUIWindow.DrawButton("Browse")) {
            Path = FSystem.ToRelativePath(EditorUtility.OpenFolderPanel("Data Folder", Path, ""));
            ValidatePath();
        }
        GUIWindow.CloseLine();

        GUIWindow.StartNewLine();
        if (GUIWindow.DrawButton("Update Card", !isNew))
        {
            UpdateCard();
        }
        if (GUIWindow.DrawButton("Save Card"))
        {
            SaveCard();
        }
        if (GUIWindow.DrawButton("Clear Card"))
        {
            ClearCard();
        }
        if (GUIWindow.DrawButton("Delete Card", !isNew))
        {
            DeleteCard();
        }
        GUIWindow.CloseLine();
    }
}
