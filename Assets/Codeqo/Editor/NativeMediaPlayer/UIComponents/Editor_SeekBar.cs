using UnityEngine;
using UnityEditor;
using CodeqoEditor;

[CustomEditor(typeof(SeekBar))]
public class Editor_SeekBar : Editor
{
    SerializedProperty _slider;
    SerializedProperty _updateInterval;
    SerializedProperty _currentPositionText;
    SerializedProperty _durationText;
    SerializedProperty _timeFormat;
    SerializedProperty _showMillisec;

    private void OnEnable()
    {
        _slider = serializedObject.FindProperty("slider");
        _updateInterval = serializedObject.FindProperty("updateInterval");
        _currentPositionText = serializedObject.FindProperty("currentPositionText");
        _durationText = serializedObject.FindProperty("durationText");
        _timeFormat = serializedObject.FindProperty("timeFormat");
        _showMillisec = serializedObject.FindProperty("showMillisec");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/ic_seekbar.png", typeof(Texture2D));

        CUILayout.BoxUILayout("Seek Bar", () => {
            EditorGUILayout.PropertyField(_slider, new GUIContent("SeekBar Slider"));
            EditorGUILayout.PropertyField(_updateInterval);
            EditorGUILayout.PropertyField(_currentPositionText);
            EditorGUILayout.PropertyField(_durationText);
            EditorGUILayout.PropertyField(_timeFormat);
            EditorGUILayout.PropertyField(_showMillisec);
            EditorGUILayout.HelpBox("Add a slider to use as a Seek Bar(Seek Slider). " +
                "You can add custom events to the slider, and they will trigger when audio position changes.",
                MessageType.Info);
        }, texture);

        serializedObject.ApplyModifiedProperties();
    }
}
