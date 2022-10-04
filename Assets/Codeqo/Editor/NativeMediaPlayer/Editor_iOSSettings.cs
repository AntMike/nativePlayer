using UnityEngine;
using UnityEditor;
using CodeqoEditor;

[CustomEditor(typeof(iOSSettings))]
public class Editor_iOSSettings : Editor
{
    SerializedProperty _useStop;
    SerializedProperty _useSkipToNext;
    SerializedProperty _useSkipToPrevious;
    SerializedProperty _useFastForward;
    SerializedProperty _useRewind;
    SerializedProperty _useSeekBar;

    private void OnEnable()
    {
        _useStop = serializedObject.FindProperty("useStop");
        _useSkipToNext = serializedObject.FindProperty("useSkipToNext");
        _useSkipToPrevious = serializedObject.FindProperty("useSkipToPrevious");
        _useFastForward = serializedObject.FindProperty("useFastForward");
        _useRewind = serializedObject.FindProperty("useRewind");
        _useSeekBar = serializedObject.FindProperty("useSeekBar");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.BeginVertical(CUILayout.DefaultSkin.box);
        GUILayout.Space(40);
        CUILayout.CenteredLabel(new GUIContent("iOS Preview Coming Soon"));
        GUILayout.Space(40);
        EditorGUILayout.LabelField("Supports iOS 13+.");
        EditorGUILayout.LabelField("Earlier versions won't toggle Play/Pause button on Unity side.");
        GUILayout.EndVertical();

        CUILayout.HeaderFoldoutGroup("Media Actions", () =>
        {
            int actionCount = 1;

            if (_useSkipToPrevious.boolValue) actionCount++;
            if (_useRewind.boolValue) actionCount++;
            if (_useStop.boolValue) actionCount++;
            if (_useFastForward.boolValue) actionCount++;
            if (_useSkipToNext.boolValue) actionCount++;

            GUILayout.BeginVertical(CUILayout.Box());
            GUILayout.BeginHorizontal();
            MediaActionPropertyField(_useSkipToPrevious, actionCount);
            MediaActionPropertyField(_useSkipToNext, actionCount);
            MediaActionPropertyField(_useRewind, actionCount);
            MediaActionPropertyField(_useFastForward, actionCount);
            MediaActionPropertyField(_useStop, actionCount);
            GUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Select up to 4 actions that will be available on native remote controls." +
            "Please note that Play/Pause action will automatically be included.", MessageType.Info);
            GUILayout.EndVertical();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(_useSeekBar);
        });

        serializedObject.ApplyModifiedProperties();
    }

    private void MediaActionPropertyField(SerializedProperty p, int count)
    {
        EditorGUI.BeginDisabledGroup(!p.boolValue && count > 4);
        p.boolValue = GUILayout.Toggle(p.boolValue, CUILayoutMediaUtility.GetIconTexture(CUILayoutMediaUtility.GetMediaActionIndex(p.displayName)), CUILayout.DefaultSkin.toggle);
        EditorGUI.EndDisabledGroup();
    }
}
