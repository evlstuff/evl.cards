using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorUtilities
{
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
