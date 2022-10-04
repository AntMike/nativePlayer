using UnityEngine;
using UnityEditor;
using CodeqoEditor;

[CustomEditor(typeof(VolumeBar))]
public class VolumeBarInspector : Editor
{
    VolumeBar script;
    SerializedProperty _slider;
    SerializedProperty _mute;

    private void OnEnable()
    {
        script = (VolumeBar)target;
        _slider = serializedObject.FindProperty("slider");
        _mute = serializedObject.FindProperty("_mute");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/ic_volumebar.png", typeof(Texture2D));

        CUILayout.BoxUILayout("Volume Bar", () => {
            EditorGUILayout.PropertyField(_slider, new GUIContent("Volume Slider"));
            EditorGUILayout.PropertyField(_mute, new GUIContent("Mute Audio"));

            EditorGUILayout.HelpBox("Add a slider to use as a Volume Bar(Volume Slider). " +
                "You can add custom events to the slider, and they will trigger when audio volume changes.", 
                MessageType.Info);
        }, texture);

        if (script.mute != _mute.boolValue) script.mute = _mute.boolValue;

        serializedObject.ApplyModifiedProperties();
    }
}
