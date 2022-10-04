using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace CodeqoEditor
{
    public class CUILayout
    {              
        static string GetGUISkinsPath()
        {
            if (Directory.Exists("Assets/Codeqo/GUISkins"))
            {
                return "Assets/Codeqo/GUISkins/";
            }
            else
            {
                string[] dirs = Directory.GetDirectories(Application.dataPath);
                string path = "";

                foreach (string dir in dirs)
                {
                    if (dir.Contains("Codeqo/GUISkins")) path = dir;
                }

                if (path != "")
                {
                    return path;
                }
                else
                {
                    Debug.LogError("Codeqo plugins failed to locate GUISkins folder. Please reinstall the plugin.");
                    return "";
                }
            }
        }

        public static GUISkin DefaultSkin
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                {
                    return (GUISkin)AssetDatabase.LoadAssetAtPath(GetGUISkinsPath() + "CSkin_Dark.guiskin", typeof(GUISkin));
                }
                else
                {
                    return (GUISkin)AssetDatabase.LoadAssetAtPath(GetGUISkinsPath() + "CSkin_Light.guiskin", typeof(GUISkin));
                }
            }
        }

        static Texture2D GetBoxTexture()
        {
            if (EditorGUIUtility.isProSkin) return (Texture2D)AssetDatabase.LoadAssetAtPath(GetGUISkinsPath() + "section-box-dark.psd", typeof(Texture2D));
            else return (Texture2D)AssetDatabase.LoadAssetAtPath(GetGUISkinsPath() + "section-box-light.psd", typeof(Texture2D));
        }

        public static Rect GetHeaderRect(Rect r, float indent = 10, float margin = 6, float width = 22, float height = 22)
        {
            return new Rect(r.position.x + indent, r.position.y + margin, width, height);
        }

        public static void DrawHorizontalLine(int thickness = 1, int padding = 14)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 5;
            r.width += 10;
            EditorGUI.DrawRect(r, new Color(99 / 255f, 99 / 255f, 99 / 255f));
        }

        public static void HeaderFoldoutGroup(string label, Action callback)
        {
            bool b = PlayerPrefs.GetInt(label, 1) == 1;

            Texture2D _ic_config = (Texture2D)AssetDatabase.LoadAssetAtPath(GetGUISkinsPath() + "header_icon_config.psd", typeof(Texture2D));
            Rect r = (Rect)EditorGUILayout.BeginVertical(DefaultSkin.box);

            /* Header */
            GUI.DrawTexture(GetHeaderRect(r), _ic_config);
            GUI.skin.label.fontSize = 14;
            GUI.Label(GetHeaderRect(r, indent: 40, width: r.width), label.ToUpper());
            GUI.skin.label.fontSize = 10;
            GUILayout.Space(20);
            b = EditorGUI.Foldout(GetHeaderRect(r, indent: r.width - 10), b, GUIContent.none);

            if (b)
            {
                if (Selection.activeTransform)
                {
                    DrawHorizontalLine();
                    GUILayout.Space(-5);
                    callback?.Invoke();
                }

                if (!Selection.activeTransform)
                {
                    b = false;
                }
            }

            EditorGUILayout.EndVertical();
            PlayerPrefs.SetInt(label, b ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static void BoxUILayout(string label, Action callback, Texture2D texture = null)
        {
            Rect r = (Rect)EditorGUILayout.BeginVertical(DefaultSkin.box);

            if (texture == null) texture = (Texture2D)AssetDatabase.LoadAssetAtPath(GetGUISkinsPath() + "header_icon_config.psd", typeof(Texture2D));

            /* Header */
            GUI.DrawTexture(new Rect(r.position.x + 10, r.position.y + 5, 24, 24), texture);
            GUI.skin.label.fontSize = 14;
            GUI.Label(GetHeaderRect(r, indent: 40, width: r.width), label.ToUpper());
            GUI.skin.label.fontSize = 10;
            GUILayout.Space(30);

            //DrawHorizontalLine();
            //GUILayout.Space(-5);
            callback?.Invoke();

            EditorGUILayout.EndVertical();
        }

        public static void BoolPropertyField(SerializedProperty p)
        {
            GUILayout.BeginHorizontal();
            GUI.skin.label.fontSize = 12;
            EditorGUILayout.PropertyField(p, GUIContent.none, GUILayout.Width(10));
            GUILayout.Label(new GUIContent(p.displayName), GUILayout.MinWidth(10));
            GUILayout.EndHorizontal();
        }

        public static void BoolPropertyField(SerializedProperty p, string label)
        {
            GUILayout.BeginHorizontal();
            GUI.skin.label.fontSize = 12;
            EditorGUILayout.PropertyField(p, GUIContent.none, GUILayout.Width(10));
            GUILayout.Label(new GUIContent(label), GUILayout.MinWidth(10));
            GUILayout.EndHorizontal();
        }

        public static void BoolPropertyField(SerializedProperty p, string label, int width)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(p, GUIContent.none, GUILayout.Width(10));
            GUILayout.Label(new GUIContent(label), GUILayout.MinWidth(10), GUILayout.Width(width));
            GUILayout.EndHorizontal();
        }

        public static void SpritePropertyField(SerializedProperty p, int size, int topMargin)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(topMargin);
            p.objectReferenceValue = EditorGUILayout.ObjectField(p.objectReferenceValue, typeof(Sprite), false, new GUILayoutOption[] {
                    GUILayout.Width(size),
                    GUILayout.Height(size)
                }); ;
            GUILayout.EndVertical();
        }

        public static void ControlledPropertyField(SerializedProperty p, string label, int labelWidth, int propertyMinWidth)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            EditorGUILayout.PropertyField(p, GUIContent.none, GUILayout.MinWidth(propertyMinWidth));
            GUILayout.EndHorizontal();
        }

        public static void ControlledPropertyField(SerializedProperty p, string label, int labelWidth)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            EditorGUILayout.PropertyField(p, GUIContent.none);
            GUILayout.EndHorizontal();
        }

        public static void ControlledPropertyField(SerializedProperty p, int labelWidth)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(p.displayName, GUILayout.Width(labelWidth));
            EditorGUILayout.PropertyField(p, GUIContent.none);
            GUILayout.EndHorizontal();
        }

        public static void DoubleHeightPropertyField(SerializedProperty p, string label, int labelWidth)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth), GUILayout.Height(40));
            EditorGUILayout.PropertyField(p, GUIContent.none, GUILayout.Height(40));
            GUILayout.EndHorizontal();
        }

        public static void ControlledBoolPropertyField(SerializedProperty p, string label, int labelWidth)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            BoolPropertyField(p, label);
            GUILayout.EndHorizontal();
        }

        public static GUIStyle Box()
        {
            GUIStyle box = new GUIStyle();
            Texture2D boxTex = GetBoxTexture();
            box.border = new RectOffset(10, 10, 10, 10);
            box.margin = new RectOffset(0, 0, 0, 0);
            box.padding = new RectOffset(6, 6, 6, 6);
            box.normal.background = boxTex;
            return box;
        }

        public static GUIStyle BoxLabel()
        {
            GUIStyle box = new GUIStyle();
            Texture2D boxTex = GetBoxTexture();
            box.border = new RectOffset(10, 10, 10, 10);
            box.margin = new RectOffset(0, 0, 0, 0);
            box.padding = new RectOffset(6, 6, 0, 6);
            box.normal.background = boxTex;
            return box;
        }

        public static void TwoColorsPropertyField(SerializedProperty c1, SerializedProperty c2)
        {
            GUILayout.BeginHorizontal();
            int left = 60;
            int right = 20;
            EditorGUILayout.PropertyField(c1, GUIContent.none, GUILayout.Width(left));
            EditorGUILayout.LabelField(c1.displayName, GUILayout.MinWidth(right));
            EditorGUILayout.PropertyField(c2, GUIContent.none, GUILayout.Width(left));
            EditorGUILayout.LabelField(c2.displayName, GUILayout.MinWidth(right));
            GUILayout.EndHorizontal();
        }

        public static void InfoVisitButton(string label, string url)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.HelpBox(
                "More information about " + label
                , MessageType.Info);

            if (GUILayout.Button("Visit", new GUILayoutOption[] {
                GUILayout.Width(50),
                GUILayout.ExpandHeight(true)
                }))
            {
                Application.OpenURL(url);
            }

            GUILayout.EndHorizontal();
        }

        public static void BoxedLabel(GUIContent label)
        {
            GUILayout.BeginHorizontal(DefaultSkin.box);
            GUILayout.Label(label);
            GUILayout.EndHorizontal();
        }

        public static void CenteredLabel(GUIContent label, int fontSize = 10)
        {
            int size = GUI.skin.label.fontSize;
            GUI.skin.label.fontSize = fontSize;
            var centeredStyle = new GUIStyle();
            centeredStyle.alignment = TextAnchor.UpperCenter;
            centeredStyle.normal.textColor = Color.black;
            Rect r = EditorGUILayout.GetControlRect();
            r.x = EditorGUIUtility.currentViewWidth / 2 - r.width / 2;           
            GUI.Label(r, label, centeredStyle);
            GUI.skin.label.fontSize = size;
            centeredStyle.alignment = TextAnchor.UpperLeft;
        }

        public static void ColoredLabelField(Rect rect, string label, Color color, bool bold = true)
        {
            var colorStyle = new GUIStyle();
            if (bold) colorStyle = EditorStyles.boldLabel;
            colorStyle.normal.textColor = color;
            EditorGUI.LabelField(rect, label, colorStyle);
        }
    }
}

