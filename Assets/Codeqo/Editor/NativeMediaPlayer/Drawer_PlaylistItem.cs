using UnityEngine;
using UnityEditor;
using CodeqoEditor;
using System.IO;
using Codeqo.NativeMediaPlayer;

[CustomPropertyDrawer(typeof(MediaItem))]
public class Drawer_MediaItem : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {        
        /* Sync Id & Metadata Type */
        SerializedProperty _index = property.FindPropertyRelative("Id");
        SerializedProperty _uriType = property.FindPropertyRelative("Path");

        bool playlist = true;
        string[] args = label.text.Split('_');
        int.TryParse(args[0], out int _itemId);
        if (_itemId != _index.intValue) _index.intValue = _itemId;

        if (args.Length > 1)
        {
            int.TryParse(args[1], out int _uriTypeValue);
            _uriType.intValue = _uriTypeValue;
            playlist = false;
        }

        /* Metadata Type */
        SerializedProperty _metadataType = property.FindPropertyRelative("MetadataType");
        SerializedProperty _artworkType = property.FindPropertyRelative("ArtworkType");
        SerializedProperty _uri = property.FindPropertyRelative("MediaUri");

        /* Metadata */
        SerializedProperty _mediaMetadata = property.FindPropertyRelative("MediaMetadata");      
        SerializedProperty _albumTitle = _mediaMetadata.FindPropertyRelative("AlbumTitle");
        SerializedProperty _albumArtist = _mediaMetadata.FindPropertyRelative("AlbumArtist");
        SerializedProperty _title = _mediaMetadata.FindPropertyRelative("Title");
        SerializedProperty _art = _mediaMetadata.FindPropertyRelative("Artwork");
        SerializedProperty _artist = _mediaMetadata.FindPropertyRelative("Artist");
        SerializedProperty _genre = _mediaMetadata.FindPropertyRelative("Genre");
        SerializedProperty _releaseDate = _mediaMetadata.FindPropertyRelative("ReleaseDate");

        /* Begin GUI */
        EditorGUI.BeginProperty(position, label, property);    

        GUILayout.Space(-20);

        GUILayout.BeginVertical(CUILayout.Box());

        /* Title (Filename) */
        GUILayout.BeginHorizontal();
        GUI.skin.label.fontSize = 12;

        string filename;
        try
        {
            filename = Path.GetFileName(_uri.stringValue);
        }
        catch { filename = "error"; }

        Color color = new Color(66 / 255f, 133 / 255f, 243 / 255f);
        CUILayout.ColoredLabelField(EditorGUILayout.GetControlRect(GUILayout.Width(30)), "Id#" + _index.intValue, color);
        GUI.Label(EditorGUILayout.GetControlRect(GUILayout.MinWidth(10), GUILayout.MaxWidth(400)), filename);

        if (GUILayout.Button("✕", new GUILayoutOption[] {
            GUILayout.Width(20),
            GUILayout.Height(20)
        }))
        {
            property.DeleteCommand();
            return;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        /* Get Artwork */
        GUILayout.BeginVertical();
        EditorGUI.BeginDisabledGroup(_artworkType.enumValueIndex == 0);
        CUILayout.SpritePropertyField(_art, 90, 5);
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(_artworkType.enumValueIndex == 1);

        if (_metadataType.enumValueIndex == 1)
        {
            _artworkType.enumValueIndex = 1;
        }

        EditorGUILayout.PropertyField(_artworkType, GUIContent.none, GUILayout.Width(90), GUILayout.ExpandWidth(false));
        EditorGUI.EndDisabledGroup();
        GUILayout.EndVertical();

        GUILayout.BeginVertical(GUILayout.MaxWidth(500), GUILayout.ExpandWidth(true));

        EditorGUI.BeginDisabledGroup(!playlist);
        EditorGUILayout.PropertyField(_uriType, GUIContent.none);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(_metadataType, GUIContent.none);

        if (_uriType.intValue != 1)
        {
            SerializedProperty _sa = property.FindPropertyRelative("StreamingAsset");
            EditorGUILayout.HelpBox("Put in a file from StreamingAssets folder below", MessageType.None);
            EditorGUILayout.PropertyField(_sa, GUIContent.none, GUILayout.Height(50), GUILayout.ExpandWidth(true));

            if (_sa.objectReferenceValue != null)
            {
                string assetPath = AssetDatabase.GetAssetPath(_sa.objectReferenceValue.GetInstanceID());
                _uri.stringValue = Path.GetFileName(assetPath);
            }
        }
        else
        {
            EditorStyles.textField.wordWrap = true;
            EditorGUILayout.HelpBox("Enter URL below", MessageType.None);
            EditorGUILayout.PropertyField(_uri, GUIContent.none, GUILayout.Height(50), GUILayout.ExpandWidth(true));
        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        /* Custom Metadata */
        if (_metadataType.intValue == 1)
        {
            GUILayout.BeginHorizontal(CUILayout.Box());
            GUILayout.BeginVertical();
            int _width = 80;
            CUILayout.ControlledPropertyField(_title, "Track Title", _width);
            CUILayout.ControlledPropertyField(_artist, "Track Artist", _width);
            CUILayout.ControlledPropertyField(_genre, "Track Genre", _width);
            GUILayout.EndVertical();
            GUILayout.Space(6);
            GUILayout.BeginVertical();
            CUILayout.ControlledPropertyField(_albumTitle, "Album Title", _width);
            CUILayout.ControlledPropertyField(_albumArtist, "Album Artist", _width);
            CUILayout.ControlledPropertyField(_releaseDate, "Release Date", _width);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(6);
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        GUILayout.EndVertical();
        EditorGUI.EndProperty();
    }
}