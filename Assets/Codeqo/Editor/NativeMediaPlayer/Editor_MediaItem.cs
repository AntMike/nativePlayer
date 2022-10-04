using UnityEditor;
using Codeqo.NativeMediaPlayer;

[CustomEditor(typeof(UnityMediaItem))]
public class Editor_MediaItem : Editor
{
    SerializedProperty _mediaItem;

    private void OnEnable()
    {
        _mediaItem = serializedObject.FindProperty("MediaItem");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_mediaItem);

        serializedObject.ApplyModifiedProperties();
    }

}