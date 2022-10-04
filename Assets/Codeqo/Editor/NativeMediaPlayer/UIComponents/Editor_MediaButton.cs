using UnityEngine;
using UnityEditor;
using CodeqoEditor;
using Codeqo.NativeMediaPlayer.UI;

[CustomEditor(typeof(MediaButton))]
public class Editor_MediaButton : Editor
{
    MediaButton _script;
    SerializedProperty _buttonType;
    SerializedProperty _image;
    SerializedProperty _sprites;
    SerializedProperty _event;
    SerializedProperty _isToggle;
    SerializedProperty _disableOnCondition;
    SerializedProperty _darkMode;
    SerializedProperty _isInteractable;

    private void OnEnable()
    {
        _script = (MediaButton)target;
        _buttonType = serializedObject.FindProperty("buttonType");
        _image = serializedObject.FindProperty("image");
        _sprites = serializedObject.FindProperty("sprites");
        _event = serializedObject.FindProperty("extraEvent");
        _isToggle = serializedObject.FindProperty("isToggle");
        _disableOnCondition = serializedObject.FindProperty("disableOnCondition");
        _darkMode = serializedObject.FindProperty("darkMode");
        _isInteractable = serializedObject.FindProperty("isInteractable");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        string btnLabel = _buttonType.enumDisplayNames[(short)_script.buttonType];

        CUILayout.BoxUILayout(btnLabel, () => {
            EditorGUILayout.PropertyField(_buttonType);
            EditorGUILayout.PropertyField(_image, new GUIContent(btnLabel + " Image"));
            ExtraFields(_script.buttonType);
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(_event, new GUIContent("On " + btnLabel + " Clicked"));
            EditorGUILayout.PropertyField(_isInteractable);
            EditorGUILayout.PropertyField(_darkMode);
        }, GetTexture(_script.buttonType));

        serializedObject.ApplyModifiedProperties();
    }

    private void ExtraFields(ButtonType btn)
    {
        switch (btn)
        {
            case ButtonType.PlayPauseToggle:
                _sprites.arraySize = 2;
                _isToggle.boolValue = true;
                EditorGUILayout.PropertyField(_sprites.GetArrayElementAtIndex(0), new GUIContent("Play Icon Sprite"));
                EditorGUILayout.PropertyField(_sprites.GetArrayElementAtIndex(1), new GUIContent("Pause Icon Sprite"));
                break;

            case ButtonType.NextButton:
                EditorGUILayout.Space(5);
                CUILayout.BoolPropertyField(_disableOnCondition, "Disable When Next Media Item Doesn't Exist");
                break;

            case ButtonType.PreviousButton:
                EditorGUILayout.Space(5);
                CUILayout.BoolPropertyField(_disableOnCondition, "Disable When Previous Media Item Doesn't Exist");
                break;

            case ButtonType.LoopOneAllToggle:
                _sprites.arraySize = 2;
                _isToggle.boolValue = true;
                EditorGUILayout.PropertyField(_sprites.GetArrayElementAtIndex(0), new GUIContent("Loop One Icon Sprite"));
                EditorGUILayout.PropertyField(_sprites.GetArrayElementAtIndex(1), new GUIContent("Loop All Icon Sprite"));
                break;

            case ButtonType.LoopOneButton:
            case ButtonType.LoopAllButton:
            case ButtonType.ShuffleButton:
                _isToggle.boolValue = true;
                break;
        }
    }

    private Texture2D GetTexture(ButtonType btn)
    {
        switch (btn)
        {
            case ButtonType.PlayPauseToggle:
            case ButtonType.PlayButton:
                return (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/ic_play_arrow_black_36dp.png", typeof(Texture2D));
            case ButtonType.PauseButton:
                return (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/ic_pause_black_36dp.png", typeof(Texture2D));
            case ButtonType.StopButton:
                return (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/ic_stop_black_36dp.png", typeof(Texture2D));
            case ButtonType.FastForwardButton:
                return (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/ic_fast_forward_black_36dp.png", typeof(Texture2D));
            case ButtonType.RewindButton:
                return (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/ic_fast_rewind_black_36dp.png", typeof(Texture2D));
            case ButtonType.NextButton:
                return (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/ic_skip_next_black_36dp.png", typeof(Texture2D));
            case ButtonType.PreviousButton:
                return (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/ic_skip_previous_black_36dp.png", typeof(Texture2D));
            case ButtonType.LoopOneAllToggle:
                return (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/baseline_repeat_black_36.png", typeof(Texture2D));
            case ButtonType.LoopOneButton:
                return (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/baseline_repeat_one_black_36.png", typeof(Texture2D));
            case ButtonType.LoopAllButton:
                return (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/baseline_repeat_black_36.png", typeof(Texture2D));
            case ButtonType.ShuffleButton:
                return (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/Icons/baseline_shuffle_black_36.png", typeof(Texture2D));
            default: return null;
        }
    }
}
