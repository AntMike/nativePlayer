using UnityEngine;
using UnityEditor;
using CodeqoEditor;
using Codeqo.NativeMediaPlayer;

[CustomEditor(typeof(NMPSettings))]
public class Editor_NMPSettings : Editor
{
    SerializedProperty _mediaSource;
    SerializedProperty _autoInit;
    SerializedProperty _autoSave;
    SerializedProperty _useRemoteCommands;
    SerializedProperty _playOnAppStart;
    SerializedProperty _iosSettings;
    SerializedProperty _androidSettings;
    SerializedProperty _repeatMode;
    SerializedProperty _seekIncrement;
    SerializedProperty _preBufferDuration;
    SerializedProperty _listener;
    SerializedProperty _addIndexType;
    SerializedProperty _albumTitle;
    SerializedProperty _albumArtist;
    SerializedProperty _title;
    SerializedProperty _artist;
    SerializedProperty _startIndex;


    private void OnEnable()
    {
        _autoInit = serializedObject.FindProperty("autoInit");
        _autoSave = serializedObject.FindProperty("autoSave");
        _useRemoteCommands = serializedObject.FindProperty("useRemoteCommands");
        _playOnAppStart = serializedObject.FindProperty("playOnAppStart");
        _iosSettings = serializedObject.FindProperty("iosSettings");
        _androidSettings = serializedObject.FindProperty("androidSettings");
        _mediaSource = serializedObject.FindProperty("source");
        _listener = serializedObject.FindProperty("listener");
        _repeatMode = serializedObject.FindProperty("repeatMode");
        _seekIncrement = serializedObject.FindProperty("seekIncrement");
        _preBufferDuration = serializedObject.FindProperty("preBufferDuration");
        _addIndexType = serializedObject.FindProperty("addIndexType");
        _albumTitle = serializedObject.FindProperty("albumTitle");
        _albumArtist = serializedObject.FindProperty("albumArtist");
        _title = serializedObject.FindProperty("title");
        _artist = serializedObject.FindProperty("artist");
        _startIndex = serializedObject.FindProperty("startIndex");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CUILayout.HeaderFoldoutGroup("General Configurations", () =>
        {
            EditorGUILayout.LabelField(new GUIContent("Core Components"));
            GUILayout.BeginVertical(CUILayout.Box());
            EditorGUILayout.PropertyField(_mediaSource, new GUIContent("Media Source"));
            EditorGUILayout.PropertyField(_listener, new GUIContent("Media Event Listener"));
            if (_useRemoteCommands.boolValue)
            {
                EditorGUILayout.PropertyField(_androidSettings);
                EditorGUILayout.PropertyField(_iosSettings, new GUIContent("iOS Settings"));
            }
            GUILayout.EndVertical();

            EditorGUILayout.LabelField(new GUIContent("On App Start"));
            GUILayout.BeginVertical(CUILayout.Box());
            EditorGUILayout.PropertyField(_autoInit);
            EditorGUILayout.PropertyField(_autoSave);
            EditorGUILayout.PropertyField(_playOnAppStart);
            EditorGUILayout.PropertyField(_useRemoteCommands, new GUIContent("Use Remote Commands", "Enable this to manage audio from background on mobile platforms. Android uses [MediaStyleNotification] and iOS uses [MPRemoteCommandCenter]"));
            GUILayout.EndVertical();
            EditorGUILayout.HelpBox("AutoSave will periodically save the values below:" +
                                    "\nVolume, RepeatMode, ShuffleModeEnabled, CurrentMediaItemIndex", MessageType.Info);
        });

        CUILayout.HeaderFoldoutGroup("Default Values", () =>
        {
            EditorGUILayout.PropertyField(_repeatMode);
            EditorGUILayout.PropertyField(_seekIncrement, new GUIContent("Seek Increment (in Sec)"));
            EditorGUILayout.PropertyField(_preBufferDuration, new GUIContent("Pre Buffer Duration (in Sec)"));
            EditorGUILayout.PropertyField(_startIndex);
            if (GUILayout.Button("Remove All Saved Data"))
            {
                ClearPlayerPrefs();
            }
        });

        CUILayout.HeaderFoldoutGroup("Default Media Metadata", () =>
        {
            EditorGUILayout.PropertyField(_addIndexType);
            EditorGUILayout.PropertyField(_albumTitle);
            EditorGUILayout.PropertyField(_albumArtist);
            EditorGUILayout.PropertyField(_title);
            EditorGUILayout.PropertyField(_artist);
        });

        CUILayout.InfoVisitButton("Native Media Player", "https://johann-song.gitbook.io/codeqos-native-plugins/native-media-player/introduction");

        EditorGUILayout.Space(10);

        serializedObject.ApplyModifiedProperties();
    }
    private void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
