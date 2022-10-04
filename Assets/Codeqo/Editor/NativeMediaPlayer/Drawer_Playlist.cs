using UnityEngine;
using UnityEditor;
using CodeqoEditor;
using Codeqo.NativeMediaPlayer;

[CustomPropertyDrawer(typeof(Playlist))]
public class Drawer_Playlist : PropertyDrawer
{
    SerializedProperty _list;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        _list = property.FindPropertyRelative("MediaItems");
        SerializedProperty _uriType = property.FindPropertyRelative("Path");
        SerializedProperty _title = property.FindPropertyRelative("Title");
        SerializedProperty _artist = property.FindPropertyRelative("Artist");

        /* Begin GUI */
        EditorGUI.BeginProperty(position, label, property);

        GUILayout.Space(-20);
        GUILayout.BeginVertical(CUILayout.Box());
        EditorGUILayout.PropertyField(_title);
        EditorGUILayout.PropertyField(_artist);
        EditorGUILayout.PropertyField(_uriType, new GUIContent("Uri Type"));
        GUILayout.EndVertical();

        if (_list.arraySize > 0)
        {
            for (int i = 0; i < _list.arraySize; i++)
            {
                EditorGUILayout.PropertyField(_list.GetArrayElementAtIndex(i), new GUIContent(i + "_" + _uriType.enumValueIndex));
            }
        }

        EditorGUILayout.Space(20);

        Vector2 pos = new Vector2((EditorGUIUtility.currentViewWidth / 2) - 100f, EditorGUILayout.GetControlRect().y);
        Vector2 size = new Vector2(180f, 40f);
        Rect r = new Rect(pos, size);

        if (GUI.Button(r, "Add a new media item"))
        {
            AddMediaItem();
        }

        EditorGUILayout.Space(40);

        EditorGUI.EndProperty();
    }

    private void AddMediaItem()
    {
        _list.arraySize++;
        //UnityPlaylist _unityPlaylist = target as UnityPlaylist;
        //Playlist _playlist = target as Playlist;
        //_playlist.AddMediaItem();
    }

    public void DeleteMediaItem(int id)
    {
        _list.DeleteArrayElementAtIndex(id);
        Debug.Log("MediaItem #" + id + " has been deleted");
    }
}