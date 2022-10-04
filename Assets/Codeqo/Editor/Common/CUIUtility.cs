using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Xml.Linq;
using System.Linq;
using System;

namespace CodeqoEditor
{
    public class CUIUtility
    {
        public static void XmlValueChange(string _index, string _value)
        {
            string xmlPath = "Assets/Codeqo/Plugins/Android/res/values/strings.xml";
            var doc = XDocument.Load(xmlPath);

            try
            {
                var valueToChange =
                    (from p in doc.Descendants("string")
                     where p.Attribute("name").Value == _index
                     select p);

                foreach (XElement element in valueToChange)
                {
                    if (element.Value != _value)
                    {
                        element.Value = _value;
                        doc.Save(xmlPath);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public static void DrawCircle(Rect r, Texture2D image = null, float margin = 6)
        {
            Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/circle_android12.psd", typeof(Texture2D));
            GUI.DrawTexture(r, tex);            
            r.width -= margin;
            r.height -= margin;
            r.x += margin / 2;
            r.y += margin / 2;
            GUI.DrawTexture(r, image);
        }

        public static Rect DrawRoundedTexture(float size, Texture2D tex)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Width(size - 2), GUILayout.Height(size - 2));
            GUI.DrawTexture(r, tex);
            r.width += 2;
            r.height += 2;
            r.x--;
            r.y--;
            GUI.Box(r, "", CUILayout.DefaultSkin.GetStyle("rounded_texture"));
            return r;
        }

        public static void OverrideSprite(SerializedProperty _obj, SerializedProperty _spr)
        {
            if (_obj.objectReferenceValue != null && _spr.objectReferenceValue == null)
            {
                GameObject _gObj = _obj.objectReferenceValue as GameObject;
                Image _img = _gObj.GetComponent<Image>();

                if (_img != null)
                {
                    _spr.objectReferenceValue = _gObj.GetComponent<Image>().sprite;
                    Debug.Log("Sprite found. This game object already has a sprite.");
                }
            }
        }

        public static System.Object GetPropertyInstance(SerializedProperty property)
        {
            string path = property.propertyPath;
            System.Object obj = property.serializedObject.targetObject;
            var type = obj.GetType();

            var fieldNames = path.Split('.');
            for (int i = 0; i < fieldNames.Length; i++)
            {
                var info = type.GetField(fieldNames[i]);
                if (info == null)
                    break;

                // Recurse down to the next nested object.
                obj = info.GetValue(obj);
                type = info.FieldType;
            }

            return obj;
        }
    }
}
