using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using EditorUtilities;

public class DeckEditorWindow : EditorWindow
{
    Deck deck;
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

    private void OnEnable()
    {
        
    }

    private void OnGUI()
    {
        string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
        string[] biuldTargets = System.Enum.GetNames(typeof(BuildTarget));

        GUILayout.Space(10);
        GUIWindow.StartNewLine();
        GUILayout.Label("Data JSON");
        if (GUIWindow.DrawButton("Update Card"))
        {
            string[] filters = { "json file", "json" };
            string path = EditorUtility.OpenFilePanelWithFilters("Select deck json", Application.dataPath, filters);
            deck = DeckUtility.FromJsonPath(path);
        }
        GUIWindow.CloseLine();

        if (deck == null) {
            return;
        }

        GUILayout.Space(10);

        GUIWindow.StartNewLine();
        GUILayout.Label("Cards Count");
        GUILayout.Label(deck.cards.Length.ToString());
        GUIWindow.CloseLine();

        GUILayout.Space(10);

        foreach (Card card in deck.cards)
        {
            GUIWindow.StartNewLine();
            GUILayout.Label(card.title);
            GUILayout.Label(card.image);
            GUILayout.Label(card.value.ToString());
            GUIWindow.CloseLine();
        }

        GUILayout.Space(10);
        GUIWindow.StartNewLine();
        bundleLabelInd = EditorGUILayout.Popup("Bundle Label", bundleLabelInd, assetBundleNames);
        buildTargetInd = EditorGUILayout.Popup(" Build Target", buildTargetInd, biuldTargets);
        buildTarget = (BuildTarget)System.Enum.Parse(typeof(BuildTarget), biuldTargets[buildTargetInd]);
        // bundleLabel = GUIWindow.DrawStringField("Bundle Label", bundleLabel);
        GUIWindow.CloseLine();

        GUIWindow.StartNewLine();
        GUILayout.Label("Target Forlder");

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.normal.textColor = isValidTempFolder ? new Color(0.2f, .5f, 0.2f) : Color.red;

        GUILayout.Label(assetBundleFolderPath != "" ? assetBundleFolderPath : "No folder selected", style);
        if (GUIWindow.DrawButton("Browse"))
        {
            assetBundleFolderPath = FSystem.ToRelativePath(EditorUtility.OpenFolderPanel("Asset Bundle Folder", assetBundleFolderPath, ""));
            isValidTempFolder = FSystem.ValidateAssetPath(assetBundleFolderPath);
        }
        GUIWindow.CloseLine();

        bool isValid = isValidTempFolder;

        GUILayout.Space(10);
        if (GUIWindow.DrawButton("Create Cards", isValid))
        {
            foreach (Card card in deck.cards)
            {
                CardUtility.CreateCardItem(card, assetBundleFolderPath, assetBundleNames[bundleLabelInd]);
            }
        }

        if (GUIWindow.DrawButton("Create Asset Bundle", isValid))
        {
            DeckUtility.CreateAssetBundle(assetBundleFolderPath, BuildAssetBundleOptions.None, buildTarget);
        }

        if (GUIWindow.DrawButton("Load Asset Bundle")) {
            string path = EditorUtility.OpenFilePanel("Asset Bundle Folder", assetBundleFolderPath, "");
            DeckUtility.LoadAssetBundle(path);
        }

        if (GUIWindow.DrawButton("Delete temp for asset bundle"))
        {
            string path = EditorUtility.OpenFilePanel("Asset Bundle Folder", assetBundleFolderPath, "");
            DeckUtility.DeleteAllAssets(path);
        }
    }
}
