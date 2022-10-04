using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using CodeqoEditor;
using Codeqo.NativeMediaPlayer;

[CustomEditor(typeof(MediaSource))]
public class Editor_MediaSource : Editor
{
    SerializedProperty _initWith;
    SerializedProperty _mediaItem;
    SerializedProperty _playlist;
    ReorderableList _playlists;

    private void OnEnable()
    {
        _initWith = serializedObject.FindProperty("initWith");
        _mediaItem = serializedObject.FindProperty("defaultMediaItem");
        _playlist = serializedObject.FindProperty("defaultPlaylist");

        _playlists = new ReorderableList(serializedObject, serializedObject.FindProperty("defaultPlaylists"));
        _playlists.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Add Playlists");
        };
        _playlists.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {

            rect.y += 2;

            SerializedProperty element = _playlists.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty playlist;
            SerializedProperty id;

            if (element.objectReferenceValue != null)
            {
                SerializedObject obj = new SerializedObject(element.objectReferenceValue);
                playlist = obj.FindProperty("Playlist");
                id = playlist.FindPropertyRelative("Id");
                id.intValue = index;
            }

            Color color = new Color(66 / 255f, 133 / 255f, 243 / 255f);
            CUILayout.ColoredLabelField(new Rect(rect.x, rect.y, 30, EditorGUIUtility.singleLineHeight), "Id#" + index, color);
            EditorGUI.PropertyField(new Rect(rect.x + 34, rect.y, rect.width - 30, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CUILayout.HeaderFoldoutGroup("Media Source", () =>
        {
            EditorGUILayout.PropertyField(_initWith, new GUIContent("Initiate Player with"));

            GUILayout.BeginVertical(CUILayout.Box());

            switch (_initWith.enumValueIndex)
            {
                case 0:
                    EditorGUILayout.PropertyField(_mediaItem);
                    break;
                case 1: 
                    EditorGUILayout.PropertyField(_playlist); 
                    break;
                case 2:
                    _playlists.DoLayoutList();
                    break;
            }

            GUILayout.EndVertical();
        });

        serializedObject.ApplyModifiedProperties();
    }
}
