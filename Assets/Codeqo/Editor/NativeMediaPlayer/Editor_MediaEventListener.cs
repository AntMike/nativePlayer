using UnityEngine;
using UnityEditor;
using CodeqoEditor;
using Codeqo.NativeMediaPlayer;

[CustomEditor(typeof(MediaEventListener))]
public class Editor_MediaEventListener : Editor
{
    SerializedProperty _onInit;
    SerializedProperty _onReady;
    SerializedProperty _onRetrieved;
    SerializedProperty _onPrepared;
    SerializedProperty _onComplete;
    SerializedProperty _onIsPlayingChangedTrue;
    SerializedProperty _onIsPlayingChangedFalse;
    SerializedProperty _onIsLoadingChangedTrue;
    SerializedProperty _onIsLoadingChangedFalse;
    SerializedProperty _onIsBufferingChangedTrue;
    SerializedProperty _onIsBufferingChangedFalse;
    SerializedProperty _onError;

    private void OnEnable()
    {
        _onInit = serializedObject.FindProperty("onInit");
        _onReady = serializedObject.FindProperty("onReady");
        _onRetrieved = serializedObject.FindProperty("onRetrieved");
        _onPrepared = serializedObject.FindProperty("onPrepared");
        _onComplete = serializedObject.FindProperty("onComplete");
        _onIsPlayingChangedTrue = serializedObject.FindProperty("onIsPlayingChangedTrue");
        _onIsPlayingChangedFalse = serializedObject.FindProperty("onIsPlayingChangedFalse");
        _onIsLoadingChangedTrue = serializedObject.FindProperty("onIsLoadingChangedTrue");
        _onIsLoadingChangedFalse = serializedObject.FindProperty("onIsLoadingChangedFalse");
        _onIsBufferingChangedTrue = serializedObject.FindProperty("onIsBufferingChangedTrue");
        _onIsBufferingChangedFalse = serializedObject.FindProperty("onIsBufferingChangedFalse");
        _onError = serializedObject.FindProperty("onError");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CUILayout.HeaderFoldoutGroup("On Init", () => {
            string tag = "Called when the player finishes its initiation";
            EditorGUILayout.PropertyField(_onInit, new GUIContent(tag));
        });

        CUILayout.HeaderFoldoutGroup("On Ready", () => {
            string tag = "Called when the playlist is successfully loaded";
            EditorGUILayout.PropertyField(_onReady, new GUIContent(tag));
        });

        CUILayout.HeaderFoldoutGroup("On Retrieved", () => {
            string tag = "Called when all media metadatas are loaded on the Unity side";
            EditorGUILayout.PropertyField(_onRetrieved, new GUIContent(tag));
        });

        CUILayout.HeaderFoldoutGroup("On Prepared", () => {
            string tag = "Called when the player is prepared to play a MediaItem";
            EditorGUILayout.PropertyField(_onPrepared, new GUIContent(tag));
        });

        CUILayout.HeaderFoldoutGroup("On Complete", () => {
            string tag = "Called when the player finishes playing a MediaItem";
            EditorGUILayout.PropertyField(_onComplete, new GUIContent(tag));
        });

        CUILayout.HeaderFoldoutGroup("On IsPlayingChanged", () => {
            string tag = "Called when the player starts playing";
            EditorGUILayout.PropertyField(_onIsPlayingChangedTrue, new GUIContent(tag));
            tag = "Called when the player pauses/stops playing";
            EditorGUILayout.PropertyField(_onIsPlayingChangedFalse, new GUIContent(tag));
        });

        CUILayout.HeaderFoldoutGroup("On IsLoadingChanged", () => {
            string tag = "Called when the player starts loading a MediaItem";
            EditorGUILayout.PropertyField(_onIsLoadingChangedTrue, new GUIContent(tag));
            tag = "Called when the player finishes loading a MediaItem";
            EditorGUILayout.PropertyField(_onIsLoadingChangedFalse, new GUIContent(tag));
        });

        CUILayout.HeaderFoldoutGroup("On IsBufferingChanged", () => {
            string tag = "Called when the player starts buffering the remote source";
            EditorGUILayout.PropertyField(_onIsBufferingChangedTrue, new GUIContent(tag));
            tag = "Called when the player stops buffering the remote source";
            EditorGUILayout.PropertyField(_onIsBufferingChangedFalse, new GUIContent(tag));
        });

        CUILayout.HeaderFoldoutGroup("On Error", () => {
            string tag = "Called when an error occurs";
            EditorGUILayout.PropertyField(_onError, new GUIContent(tag));
        });

        serializedObject.ApplyModifiedProperties();
    }


}
