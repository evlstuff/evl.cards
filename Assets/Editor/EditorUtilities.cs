using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace EditorUtilities
{
    public class DeckUtility
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

        static public Deck FromJson(string json)
        {
            return JsonUtility.FromJson<Deck>(json);
        }

        static public Deck FromJsonPath(string path)
        {
            string json = LoadJson(path);

            if (json == null) { return null; }

            return FromJson(json);
        }

        static public bool CreateDeckItem(DeckItem deck, string path)
        {
            string filePath = path + "deck.asset";
            bool exists = File.Exists(filePath);

            if (exists)
            {
                AssetDatabase.DeleteAsset(filePath);
            }

            DeckItem asset = ScriptableObject.CreateInstance<DeckItem>();
            asset.name = deck.name != null ? deck.name : "Deck";
            asset.cards = deck.cards;

            AssetDatabase.CreateAsset(asset, filePath);
            AssetDatabase.SaveAssets();

            return true;
        }

        static public bool CreateAssetBundle(string path, BuildAssetBundleOptions options, BuildTarget target) {
            string bundlePath = path + target.ToString();

            if (Directory.Exists(bundlePath))
            {
                Directory.Delete(bundlePath, true);
            }

            Directory.CreateDirectory(bundlePath);

            BuildPipeline.BuildAssetBundles(bundlePath, options, target);
            return true;
        }

        static public AssetBundle LoadAssetBundleFromFile(string path)
        {
            AssetBundle.UnloadAllAssetBundles(true);
            return AssetBundle.LoadFromFile(path);
        }

        static public Deck LoadAssetBundle(string path) {
            AssetBundle assetBundle = LoadAssetBundleFromFile(path);
            if (assetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return null;
            }

            string[] assetNames = assetBundle.GetAllAssetNames();
            Debug.Log("Asset Bundle Name: " + assetBundle.name + " has " + assetNames.Length.ToString() + " assets");


            foreach(string assetName in assetNames)
            {
                CardItem card = assetBundle.LoadAsset<CardItem>(assetName);
                if (card != null)
                {
                    Debug.Log(card.title + " with value " + card.value);
                }
                else
                {
                    Debug.Log("Cant load " + assetName);
                }
            }
            
            Deck deck = new Deck();
            //deck.cards = cards;

            return deck;
        }

        public static void DeleteAllAssets(string bundlePath)
        {
            AssetBundle assetBundle = LoadAssetBundleFromFile(bundlePath);

            if (assetBundle == null)
            {
                return;
            }

            string[] assetNames = assetBundle.GetAllAssetNames();
            foreach (string assetName in assetNames)
            {
                AssetDatabase.DeleteAsset(assetName);
            }
        }
    }

    public class CardUtility
    {
        static public bool CreateCardItem(Card card, string path, string bundleName)
        {
            string bundlePath = Path.Combine(path, bundleName + "/");
            string full = Path.GetFullPath(bundlePath);

            if (!Directory.Exists(full)) {
                Directory.CreateDirectory(full);
            }

            string pattern = @"\s";
            string name = Regex.Replace(card.title, pattern, "_").ToLower();
            string filePath = bundlePath + name + ".asset";
            bool exists = File.Exists(filePath);

            if (exists)
            {
                AssetDatabase.DeleteAsset(filePath);
            }

            CardItem asset = ScriptableObject.CreateInstance<CardItem>();
            // asset.name = name;
            asset.title = card.title;
            asset.description = card.description;
            asset.value = card.value;

            AssetDatabase.CreateAsset(asset, filePath);
            AssetDatabase.SaveAssets();

            AssetImporter.GetAtPath(filePath).SetAssetBundleNameAndVariant(bundleName, "");

            return true;
        }
    }

    public class GUIWindow : EditorWindow
    {
        public static void StartNewLine()
        {
            GUILayout.BeginHorizontal();
        }

        public static void CloseLine()
        {
            GUILayout.EndHorizontal();
        }

        public static string DrawStringField(string label, string value, bool newLine = false)
        {
            if (newLine) { StartNewLine(); }
            GUILayout.Label(label);
            string res = GUILayout.TextField(value);
            if (newLine) { CloseLine(); }

            return res;
        }

        public static int DrawIntField(string label, int value, bool newLine = false)
        {
            if (newLine) { StartNewLine(); }
            GUILayout.Label(label);
            int res = EditorGUILayout.IntField(value);
            if (newLine) { CloseLine(); }

            return res;
        }

        public static bool DrawButton(string label, bool enabled = true, bool newLine = false)
        {
            if (newLine) { StartNewLine(); }
            GUI.enabled = enabled;
            bool res = GUILayout.Button(label);
            GUI.enabled = true;
            if (newLine) { CloseLine(); }

            return res;
        }
    }

    public class FSystem
    {
        public static bool ValidateAssetPath(string path)
        {
            bool isValid = Directory.Exists(path);

            if (!isValid) { Debug.Log(path + " is not valid"); }

            return isValid;
        }
        public static string ToRelativePath(string filePath)
        {
            Uri fileUri = new Uri(filePath);

            if (!fileUri.IsAbsoluteUri) {
                return filePath;
            }

            Uri referenceUri = new Uri(Application.dataPath);

            string path = referenceUri.MakeRelativeUri(fileUri).ToString();
            MatchCollection match = (new Regex(@"./$")).Matches(path);
            bool closedWithSlash = match.Count != 0;

            if (!closedWithSlash) {
                path += @"/";
            }

            return path;
        }
    }
}
