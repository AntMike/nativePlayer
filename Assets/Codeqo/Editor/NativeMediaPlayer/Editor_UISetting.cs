using UnityEngine;
using UnityEditor;
using CodeqoEditor;

[CustomEditor(typeof(UISettings))]
public class Editor_UISetting : Editor
{
    SerializedProperty _lightModeEnabledColor;
    SerializedProperty _lightModeDisabledColor;
    SerializedProperty _darkModeEnabledColor;
    SerializedProperty _darkModeDisabledColor;

    private void OnEnable()
    {
        _lightModeEnabledColor = serializedObject.FindProperty("lightModeEnabledColor");
        _lightModeDisabledColor = serializedObject.FindProperty("lightModeDisabledColor");
        _darkModeEnabledColor = serializedObject.FindProperty("darkModeEnabledColor");
        _darkModeDisabledColor = serializedObject.FindProperty("darkModeDisabledColor");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CUILayout.HeaderFoldoutGroup("Light Mode Colors", () =>
        {
            EditorGUILayout.PropertyField(_lightModeEnabledColor, new GUIContent("Enabled Button Color"));
            EditorGUILayout.PropertyField(_lightModeDisabledColor, new GUIContent("Disabled Button Color"));
        });

        CUILayout.HeaderFoldoutGroup("Dark Mode Colors", () =>
        {
            EditorGUILayout.PropertyField(_darkModeEnabledColor, new GUIContent("Enabled Button Color"));
            EditorGUILayout.PropertyField(_darkModeDisabledColor, new GUIContent("Disabled Button Color"));
        });

        serializedObject.ApplyModifiedProperties();
    }
}  