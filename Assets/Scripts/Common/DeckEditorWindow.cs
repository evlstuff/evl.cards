using System;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEditor;

public class DeckEditorWindow : EditorWindow
{
    DeckItem deck;
    string assetBundleFolderPath;
    bool isValidTempFolder;
    string bundleLabel;
    int bundleLabelInd;
    int buildTargetInd;
    BuildTarget buildTarget;

    [MenuItem("Window/Card/Deck Editor")]
    static void Init() {
        EditorWindow.GetWindow(typeof(DeckEditorWindow), false, "Deck Editor");
    }

    public void StartNewLine()
    {
        GUILayout.BeginHorizontal();
    }

    public void CloseLine()
    {
        GUILayout.EndHorizontal();
    }

    public string DrawStringField(string label, string value, bool newLine = false)
    {
        if (newLine) { StartNewLine(); }
        GUILayout.Label(label);
        string res = GUILayout.TextField(value);
        if (newLine) { CloseLine(); }

        return res;
    }

    public int DrawIntField(string label, int value, bool newLine = false)
    {
        if (newLine) { StartNewLine(); }
        GUILayout.Label(label);
        int res = EditorGUILayout.IntField(value);
        if (newLine) { CloseLine(); }

        return res;
    }

    public bool DrawButton(string label, bool enabled = true, bool newLine = false)
    {
        if (newLine) { StartNewLine(); }
        GUI.enabled = enabled;
        bool res = GUILayout.Button(label);
        GUI.enabled = true;
        if (newLine) { CloseLine(); }

        return res;
    }

    public bool ValidateAssetPath(string path)
    {
        bool isValid = Directory.Exists(path);

        if (!isValid) { Debug.Log(path + " is not valid"); }

        return isValid;
    }
    public string ToRelativePath(string filePath)
    {
        Uri fileUri = new Uri(filePath);

        if (!fileUri.IsAbsoluteUri)
        {
            return filePath;
        }

        Uri referenceUri = new Uri(Application.dataPath);

        string path = referenceUri.MakeRelativeUri(fileUri).ToString();
        MatchCollection match = (new Regex(@"./$")).Matches(path);
        bool closedWithSlash = match.Count != 0;

        if (!closedWithSlash)
        {
            path += @"/";
        }

        return path;
    }

    private void OnEnable()
    {
        
    }

    private void OnGUI()
    {
        string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
        string[] biuldTargets = System.Enum.GetNames(typeof(BuildTarget));

        GUILayout.Space(10);
        StartNewLine();
        GUILayout.Label("Data JSON");
        if (DrawButton("Update Card"))
        {
            string[] filters = { "json file", "json" };
            string path = EditorUtility.OpenFilePanelWithFilters("Select deck json", Application.dataPath, filters);
            string json = Editor.LoadJson(path);
            deck = Editor.JsonToDeck(json);
        }
        CloseLine();

        if (deck == null) {
            return;
        }

        GUILayout.Space(10);

        StartNewLine();
        GUILayout.Label("Cards Count");
        GUILayout.Label(deck.cards.Length.ToString());
        CloseLine();

        GUILayout.Space(10);

        foreach (CardItem card in deck.cards)
        {
            StartNewLine();
            GUILayout.Label(card.title);
            //TODO:  GUILayout.Label(card.image);
            GUILayout.Label(card.value.ToString());
            CloseLine();
        }

        GUILayout.Space(10);
        StartNewLine();
        bundleLabelInd = EditorGUILayout.Popup("Bundle Label", bundleLabelInd, assetBundleNames);
        buildTargetInd = EditorGUILayout.Popup(" Build Target", buildTargetInd, biuldTargets);
        buildTarget = (BuildTarget)System.Enum.Parse(typeof(BuildTarget), biuldTargets[buildTargetInd]);
        // bundleLabel = GUIWindow.DrawStringField("Bundle Label", bundleLabel);
        CloseLine();

        StartNewLine();
        GUILayout.Label("Target Forlder");

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.normal.textColor = isValidTempFolder ? new Color(0.2f, .5f, 0.2f) : Color.red;

        GUILayout.Label(assetBundleFolderPath != "" ? assetBundleFolderPath : "No folder selected", style);
        if (DrawButton("Browse"))
        {
            assetBundleFolderPath = ToRelativePath(EditorUtility.OpenFolderPanel("Asset Bundle Folder", assetBundleFolderPath, ""));
            isValidTempFolder = ValidateAssetPath(assetBundleFolderPath);
        }
        CloseLine();

        bool isValid = isValidTempFolder;

        GUILayout.Space(10);
        if (DrawButton("Create Cards", isValid))
        {
            foreach (CardItem card in deck.cards)
            {
                string bundleName = assetBundleNames.Length > 0 ? assetBundleNames[bundleLabelInd] : null;
                Editor.SaveCardItem(card, assetBundleFolderPath, bundleName);
            }
        }

        if (DrawButton("Create Asset Bundle", isValid))
        {
            Editor.BuildAssetBundles(assetBundleFolderPath, BuildAssetBundleOptions.None, buildTarget);
        }

        if (DrawButton("Load Asset Bundle")) {
            string path = EditorUtility.OpenFilePanel("Asset Bundle Folder", assetBundleFolderPath, "");
            DeckItem bundleDeck = Editor.LoadAssetBundle(path);
            Debug.Log("Bundle loaded and consists of " + bundleDeck.cards.Length + " cards. First is  " + bundleDeck.cards[0].title);
        }

        if (DrawButton("Delete temp for asset bundle"))
        {
            string path = EditorUtility.OpenFilePanel("Asset Bundle Folder", assetBundleFolderPath, "");
            // TODO: Editor.DeleteAllAssets(path);
        }
    }
}
