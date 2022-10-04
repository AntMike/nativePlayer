using UnityEditor;
using Codeqo.NativeMediaPlayer;

[CustomEditor(typeof(UnityPlaylist))]
public class Editor_Playlist : Editor
{
    SerializedProperty _playlist;

    private void OnEnable()
    {
        _playlist = serializedObject.FindProperty("Playlist");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_playlist);

        serializedObject.ApplyModifiedProperties();
    }
}
