using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class DeckItem : ScriptableObject
{
    public CardItem[] cards;
}

public class Editor
{
    public static string LoadJson(string path)
    {
        bool exists = File.Exists(path);

        if (exists)
        {
            return File.ReadAllText(path);
        }

        return null;
    }

    static public DeckItem JsonToDeck(string json)
    {
        return JsonUtility.FromJson<DeckItem>(json);
    }

    static public AssetBundle LoadAssetBundleFromFile(string path)
    {
        AssetBundle.UnloadAllAssetBundles(true);
        return AssetBundle.LoadFromFile(path);
    }

    static public bool BuildAssetBundles(string path, BuildAssetBundleOptions options, BuildTarget target)
    {
        string bundlePath = path + target.ToString();

        if (Directory.Exists(bundlePath))
        {
            Directory.Delete(bundlePath, true);
        }

        Directory.CreateDirectory(bundlePath);

        BuildPipeline.BuildAssetBundles(bundlePath, options, target);
        return true;
    }

    static public DeckItem LoadAssetBundle(string path)
    {
        AssetBundle assetBundle = LoadAssetBundleFromFile(path);
        if (assetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return null;
        }

        string[] assetNames = assetBundle.GetAllAssetNames();

        DeckItem deck = new DeckItem();
        List<CardItem> cards = new List<CardItem>();
        foreach (string assetName in assetNames)
        {
            CardItem card = assetBundle.LoadAsset<CardItem>(assetName);

            if (card != null) {
                cards.Add(card);
            }
        }
        deck.cards = cards.ToArray();

        return deck;
    }

    static public bool SaveCardItem(CardItem card, string path, string bundleName)
    {
        bool hasBundleName = bundleName != null && bundleName != "";
        string bundle = hasBundleName ? bundleName : "default";
        string bundlePath = Path.Combine(path, bundle + "/");
        string full = Path.GetFullPath(bundlePath);

        if (!Directory.Exists(full))
        {
            Directory.CreateDirectory(full);
        }

        CardItem asset = card.ToAsset();

        string filePath = bundlePath + asset.name + ".asset";
        bool exists = File.Exists(filePath);

        if (exists)
        {
            AssetDatabase.DeleteAsset(filePath);
        }

        AssetDatabase.CreateAsset(asset, filePath);
        AssetDatabase.SaveAssets();

        AssetImporter.GetAtPath(filePath).SetAssetBundleNameAndVariant(bundle, "");

        return true;
    }
}
