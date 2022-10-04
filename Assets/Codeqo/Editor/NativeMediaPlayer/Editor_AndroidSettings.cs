using UnityEngine;
using UnityEditor;
using CodeqoEditor;
using System;
using Codeqo.NativeMediaPlayer;

[CustomEditor(typeof(AndroidSettings))]
public class Editor_AndroidSettings : Editor
{
    SerializedProperty _smallIcon;
    SerializedProperty _notificationId;
    SerializedProperty _notificationImportance;
    SerializedProperty _notificationChannelId;
    SerializedProperty _notificationChannelName;
    SerializedProperty _notificationChannelDescription;

    SerializedProperty _useStop;
    SerializedProperty _useSkipToNext;
    SerializedProperty _useSkipToPrevious;
    SerializedProperty _useFastForward;
    SerializedProperty _useRewind;
    SerializedProperty _useSkipToNextCompact;
    SerializedProperty _useSkipToPreviousCompact;
    SerializedProperty _useFastForwardCompact;
    SerializedProperty _useRewindCompact;
    SerializedProperty _useClose;

    SerializedProperty _audioUsage;
    SerializedProperty _contentType;
    SerializedProperty _allowedCapturePolicy;
    SerializedProperty _acceptsDelayedFocusGain;
    SerializedProperty _willPauseWhenDucked;
    SerializedProperty _audioFocusGain;
    SerializedProperty _audioFocusGainTransient;
    SerializedProperty _audioFocusLoss;
    SerializedProperty _audioFocusLossTransient;
    SerializedProperty _audioFocusLossTransientCanDuck;
    SerializedProperty _returnToApp;
    SerializedProperty _terminateApp;

    private void OnEnable()
    {
        _notificationId = serializedObject.FindProperty("notificationId");
        _notificationImportance = serializedObject.FindProperty("notificationImportance");
        _notificationChannelId = serializedObject.FindProperty("notificationChannelId");
        _notificationChannelName = serializedObject.FindProperty("notificationChannelName");
        _notificationChannelDescription = serializedObject.FindProperty("notificationChannelDescription");
        _smallIcon = serializedObject.FindProperty("smallIcon");

        _useStop = serializedObject.FindProperty("useStop");
        _useSkipToNext = serializedObject.FindProperty("useSkipToNext");
        _useSkipToPrevious = serializedObject.FindProperty("useSkipToPrevious");
        _useFastForward = serializedObject.FindProperty("useFastForward");
        _useRewind = serializedObject.FindProperty("useRewind");
        _useSkipToNextCompact = serializedObject.FindProperty("useSkipToNextCompact");
        _useSkipToPreviousCompact = serializedObject.FindProperty("useSkipToPreviousCompact");
        _useFastForwardCompact = serializedObject.FindProperty("useFastForwardCompact");
        _useRewindCompact = serializedObject.FindProperty("useRewindCompact");
        _useClose = serializedObject.FindProperty("useClose");

        _audioUsage = serializedObject.FindProperty("audioUsage");
        _contentType = serializedObject.FindProperty("contentType");
        _allowedCapturePolicy = serializedObject.FindProperty("allowedCapturePolicy");
        _acceptsDelayedFocusGain = serializedObject.FindProperty("acceptsDelayedFocusGain");
        _willPauseWhenDucked = serializedObject.FindProperty("willPauseWhenDucked");
        _audioFocusGain = serializedObject.FindProperty("audioFocusGain");
        _audioFocusGainTransient = serializedObject.FindProperty("audioFocusGainTransient");
        _audioFocusLoss = serializedObject.FindProperty("audioFocusLoss");
        _audioFocusLossTransient = serializedObject.FindProperty("audioFocusLossTransient");
        _audioFocusLossTransientCanDuck = serializedObject.FindProperty("audioFocusLossTransientCanDuck");

        _returnToApp = serializedObject.FindProperty("returnToAppOnNotificationClicked");
        _terminateApp = serializedObject.FindProperty("terminateAppOnNotificationDismissed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.BeginVertical(CUILayout.DefaultSkin.box);
        GetAndroid12();
        EditorGUILayout.LabelField("Supports SDK26+. Tested on SDK 26-31.");
        GUILayout.EndVertical();

        CUILayout.HeaderFoldoutGroup("Media Actions", () =>
        {
            int actionCount = 1;
            int compactCount = 1;

            if (_useSkipToPrevious.boolValue) actionCount++;
            if (_useRewind.boolValue) actionCount++;
            if (_useStop.boolValue) actionCount++;
            if (_useFastForward.boolValue) actionCount++;
            if (_useSkipToNext.boolValue) actionCount++;
            if (_useClose.boolValue) actionCount++;
            if (_useSkipToNextCompact.boolValue) compactCount++;
            if (_useSkipToPreviousCompact.boolValue) compactCount++;
            if (_useRewindCompact.boolValue) compactCount++;
            if (_useFastForwardCompact.boolValue) compactCount++;

            GUILayout.BeginVertical(CUILayout.DefaultSkin.box);
            EditorGUILayout.LabelField("Expanded View");
            GUILayout.BeginHorizontal();
            MediaActionPropertyField(_useSkipToPrevious, actionCount);
            MediaActionPropertyField(_useSkipToNext, actionCount);
            MediaActionPropertyField(_useRewind, actionCount);
            MediaActionPropertyField(_useFastForward, actionCount);
            MediaActionPropertyField(_useStop, actionCount, _useClose.boolValue);
            MediaActionPropertyField(_useClose, actionCount, _useStop.boolValue);
            GUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Select up to 4 actions that will be available on native remote controls." +
                "Please note that Play/Pause action will automatically be included.", MessageType.Info);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(CUILayout.DefaultSkin.box);
            EditorGUILayout.LabelField("Compact View");
            GUILayout.BeginHorizontal();
            MediaActionCompactPropertyField(_useSkipToPrevious, _useSkipToPreviousCompact, compactCount);
            MediaActionCompactPropertyField(_useSkipToNext, _useSkipToNextCompact, compactCount);
            MediaActionCompactPropertyField(_useRewind, _useRewindCompact, compactCount);
            MediaActionCompactPropertyField(_useFastForward, _useFastForwardCompact, compactCount);
            GUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Select up to 2 actions that will be available on Android Media Style Notification when not expanded.", MessageType.Info);
            GUILayout.EndVertical();
        });

        if (!_useSkipToNext.boolValue && _useSkipToNextCompact.boolValue)
            _useSkipToNextCompact.boolValue = false;
        if (!_useSkipToPrevious.boolValue && _useSkipToPreviousCompact.boolValue)
            _useSkipToPreviousCompact.boolValue = false;
        if (!_useFastForward.boolValue && _useFastForwardCompact.boolValue)
            _useFastForwardCompact.boolValue = false;
        if (!_useRewind.boolValue && _useRewindCompact.boolValue)
            _useRewindCompact.boolValue = false;

        CUILayout.HeaderFoldoutGroup("Notifcation Settings", () =>
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("Notification Id", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("Notification Importance", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("(SDK26+) Channel Id", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("(SDK26+) Channel Name", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("(SDK26+) Channel Decription", GUILayout.MinWidth(100));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(_notificationId, GUIContent.none, GUILayout.MinWidth(140));
            EditorGUILayout.PropertyField(_notificationImportance, GUIContent.none, GUILayout.MinWidth(140));
            EditorGUILayout.PropertyField(_notificationChannelId, GUIContent.none, GUILayout.MinWidth(140));
            EditorGUILayout.PropertyField(_notificationChannelName, GUIContent.none, GUILayout.MinWidth(140));
            EditorGUILayout.PropertyField(_notificationChannelDescription, GUIContent.none, GUILayout.MinWidth(140));
            GUILayout.EndVertical();
            GUILayout.Space(4);
            GUILayout.BeginVertical(GUI.skin.label);
            _smallIcon.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField(
                 _smallIcon.objectReferenceValue, typeof(Sprite), allowSceneObjects: true, new GUILayoutOption[] {
                GUILayout.MinWidth(74),
                GUILayout.MinHeight(74),
                GUILayout.MaxWidth(74),
                GUILayout.MaxHeight(74)
                });
            EditorGUILayout.HelpBox(new GUIContent("Small Icon"));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginVertical(CUILayout.Box());

            EditorGUILayout.LabelField("Notification Behavior", EditorStyles.boldLabel);
            CUILayout.BoolPropertyField(_returnToApp, "Return to app when notification is clicked");
            CUILayout.BoolPropertyField(_terminateApp, "Termiate app when notification is dismissed");
            EditorGUILayout.HelpBox("Dismissing notification by swiping it no longer allows callbacks with SDK30+. Use the X button instead.", MessageType.None);
            GUILayout.EndVertical();
            GUILayout.Space(5);

            EditorGUILayout.HelpBox(
                "Try changing Channel Id if you have trouble applying changes with remote actions.", MessageType.Warning);

            CUILayout.InfoVisitButton("Notification Importance", "https://developer.android.com/reference/android/app/NotificationManager");
        });

        CUILayout.HeaderFoldoutGroup("Audio Attributes", () =>
        {
            int w = 190;
            CUILayout.ControlledPropertyField(_audioUsage, w);
            CUILayout.ControlledPropertyField(_contentType, w);
            CUILayout.ControlledPropertyField(_allowedCapturePolicy, "(SDK26+) Allowed Capture Policy", w);
            CUILayout.InfoVisitButton("AudioAttributes(AudioUsage, ContentType)", "https://developer.android.com/reference/android/media/AudioAttributes");
            CUILayout.InfoVisitButton("AllowedCapturePolicy", "https://developer.android.com/reference/android/media/AudioAttributes.Builder#setAllowedCapturePolicy(int)");
        });

        CUILayout.HeaderFoldoutGroup("Audio Focus Request", () =>
        {
            CUILayout.BoolPropertyField(_acceptsDelayedFocusGain, "Marks this focus request as compatible with delayed focus");
            CUILayout.BoolPropertyField(_willPauseWhenDucked, "Declare the intended behavior of the application with regards to audio ducking");
            EditorGUILayout.LabelField(new GUIContent("Choose an action that will be executed when your app(game)"), EditorStyles.boldLabel);
            int w = 260;
            CUILayout.ControlledPropertyField(_audioFocusGain, "Gains audio focus", w);
            CUILayout.ControlledPropertyField(_audioFocusGainTransient, "Gains audio focus for a short time", w);
            CUILayout.ControlledPropertyField(_audioFocusLoss, "Loses audio focus", w);
            CUILayout.ControlledPropertyField(_audioFocusLossTransient, "Loses audio focus for a short time", w);
            CUILayout.ControlledPropertyField(_audioFocusLossTransientCanDuck, "Audio focus is reduced by the another signal", w);

            CUILayout.InfoVisitButton("AudioFocusRequest", "https://developer.android.com/reference/android/media/AudioFocusRequest.Builder");
        });

        if (_notificationChannelName.stringValue != null)
        {
            CUIUtility.XmlValueChange("ChannelName", _notificationChannelName.stringValue);
        }

        if (_notificationChannelDescription.stringValue != null)
        {
            CUIUtility.XmlValueChange("ChannelDescription", _notificationChannelDescription.stringValue);
        }

        SaveSmallIcon();
        serializedObject.ApplyModifiedProperties();
    }

    private void MediaActionPropertyField(SerializedProperty p, int count, bool state = false)
    {
        EditorGUI.BeginDisabledGroup(!p.boolValue && count > 4 || state);
        p.boolValue = GUILayout.Toggle(p.boolValue, CUILayoutMediaUtility.GetIconTexture(CUILayoutMediaUtility.GetMediaActionIndex(p.displayName)), CUILayout.DefaultSkin.toggle);
        EditorGUI.EndDisabledGroup();
    }

    private void MediaActionCompactPropertyField(SerializedProperty p1, SerializedProperty p2, int count)
    {
        EditorGUI.BeginDisabledGroup(!p1.boolValue || count > 2 && !p2.boolValue);
        p2.boolValue = GUILayout.Toggle(p2.boolValue, CUILayoutMediaUtility.GetIconTexture(CUILayoutMediaUtility.GetMediaActionIndex(p2.displayName)), CUILayout.DefaultSkin.toggle);
        EditorGUI.EndDisabledGroup();
    }

    private void DrawIcon(int value, float size, float indent = 0)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Width(size + 5), GUILayout.MinWidth(5), GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true));
        r.height = r.width = size;
        r.y += (45 - size) / 2;
        r.x += indent;
        GUI.DrawTexture(r, CUILayoutMediaUtility.GetIconTexture(value));
    }

    public void SaveSmallIcon()
    {
        if (_smallIcon.objectReferenceValue != null)
        {
            string iconPath = AssetDatabase.GetAssetPath(_smallIcon.objectReferenceValue.GetInstanceID());
            string destPath = "Assets/Codeqo/Plugins/Android/res/mipmap/small_icon.png";
            FileUtil.ReplaceFile(iconPath, destPath);
        }
    }
    private string GetTimeInFormat(TimeSpan timeSpan)
    {
        if (timeSpan != null && timeSpan.TotalSeconds > 0)
        {
            return string.Format("{0:00}:{1:00}", Mathf.FloorToInt((float)timeSpan.TotalMinutes), timeSpan.Seconds);
        }
        else
        {
            return "00:00";
        }
    }

    public void GetAndroid10(string label)
    {
        // Setting up a style
        GUIStyle _blackFont = new GUIStyle();
        _blackFont.normal.textColor = Color.black;

        // Mimic Android 10 Media Style Notification

        bool b = PlayerPrefs.GetInt(label, 1) == 1;
        Rect r = (Rect)EditorGUILayout.BeginVertical(CUILayout.DefaultSkin.GetStyle("box_filled"));

        /* Header */
        if (_smallIcon.objectReferenceValue != null)
        {
            Sprite _icon = _smallIcon.objectReferenceValue as Sprite;
            GUI.DrawTexture(CUILayout.GetHeaderRect(r, indent: 14, margin: 14, width: 20, height: 20), _icon.texture);
        }

        GUI.skin.label.fontSize = 12;
        GUI.skin.label.fontStyle = FontStyle.Normal;
        GUI.Label(CUILayout.GetHeaderRect(r, indent: 37, margin: 12, width: r.width), Application.productName);
        GUILayout.Space(20);

        GUI.skin.label.fontSize = 14;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.Label(EditorGUILayout.GetControlRect(), new GUIContent("Album Title"));
        GUI.Label(EditorGUILayout.GetControlRect(), new GUIContent("Artist"));
        EditorGUILayout.BeginHorizontal();
        if (_useSkipToPrevious.boolValue) DrawIcon(4, 35);
        if (_useRewind.boolValue) DrawIcon(6, 35);
        DrawIcon(1, 45);
        if (_useFastForward.boolValue) DrawIcon(5, 35);
        if (_useSkipToNext.boolValue) DrawIcon(3, 35);
        if (_useStop.boolValue) DrawIcon(2, 35);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(34);
        GUI.skin = CUILayout.DefaultSkin;
        GUILayout.HorizontalSlider(0, 0, 10);
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        r = EditorGUILayout.GetControlRect();
        GUI.Label(r, new GUIContent("00:00"));
        r.x += EditorGUILayout.GetControlRect().width - r.width;
        GUI.Label(r, new GUIContent("00:00"));

        GUILayout.Space(-24);
        EditorGUILayout.EndVertical();
        PlayerPrefs.SetInt(label, b ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void GetAndroid12()
    {
        // Setting up a style
        GUIStyle _blackFont = new GUIStyle();
        _blackFont.normal.textColor = Color.black;

        // Mimic Android 12 Media Style Notification

        /* Get the data first */
        Texture2D art = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/no_album_image.png", typeof(Texture2D));
        string title = "Unknown Title";
        string artist = "Unknown Artist";
        float position = 0;
        float duration = 0;
        string left = "00:00";
        string right = "00:00";

        if (Application.isPlaying && MediaPlayer.isInit)
        {
            int _id = MediaPlayer.CurrentMediaItemIndex;
            title = MediaPlayer.CurrentPlaylist.MediaItems[_id].Title;
            artist = MediaPlayer.CurrentPlaylist.MediaItems[_id].Artist;
            position = MediaPlayer.GetCurrentPosition();
            duration = MediaPlayer.GetDuration();
            left = GetTimeInFormat(TimeSpan.FromSeconds(position));
            right = GetTimeInFormat(TimeSpan.FromSeconds(duration));
            art = MediaPlayer.CurrentPlaylist.MediaItems[_id].Artwork.texture;
        }

        EditorGUILayout.BeginVertical(CUILayout.DefaultSkin.GetStyle("android12mediastylenotification"));
        Rect r;

        EditorGUILayout.BeginHorizontal();
        r = CUIUtility.DrawRoundedTexture(90, art);

        if (_smallIcon.objectReferenceValue != null)
        {
            Sprite _icon = _smallIcon.objectReferenceValue as Sprite;
            if (_icon != null) CUIUtility.DrawCircle(new Rect(r.x + r.width / 1.25f, r.y + r.height / 1.25f, 20, 20), _icon.texture);
        }

        EditorGUILayout.BeginVertical();

        /* Speaker */
        GUIStyle speaker = CUILayout.DefaultSkin.GetStyle("android12speaker");
        //speaker.margin.left = (int)EditorGUIUtility.currentViewWidth - 286;
        speaker.margin.left = (int)EditorGUIUtility.currentViewWidth - 306;
        GUILayout.BeginHorizontal(speaker);
        Rect phone = EditorGUILayout.GetControlRect(GUILayout.Width(14), GUILayout.Height(14), GUILayout.ExpandWidth(false));
        phone.y += 2;
        GUI.DrawTexture(phone, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Codeqo/GUISkins/icon_phone.psd", typeof(Texture2D)));
        _blackFont.fontSize = 12;
        _blackFont.fontStyle = FontStyle.Bold;
        _blackFont.alignment = TextAnchor.MiddleRight;
        _blackFont.stretchWidth = false;
        GUILayout.Label(new GUIContent("Phone speaker"), _blackFont);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.EndHorizontal();

        /* Track Title */
        _blackFont.fontSize = 16;
        _blackFont.fontStyle = FontStyle.Bold;
        _blackFont.alignment = TextAnchor.UpperLeft;
        GUI.Label(EditorGUILayout.GetControlRect(), new GUIContent(title), _blackFont);

        GUILayout.Space(5);

        /* Artist */
        _blackFont.fontSize = 14;
        _blackFont.fontStyle = FontStyle.Normal;
        GUI.Label(EditorGUILayout.GetControlRect(), new GUIContent(artist), _blackFont);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(14);

        /* SeekBar */
        GUI.skin.horizontalSlider = CUILayout.DefaultSkin.horizontalSlider;
        GUI.skin.horizontalSliderThumb = CUILayout.DefaultSkin.horizontalSliderThumb;
        GUILayout.HorizontalSlider(position, 0, duration);
        _blackFont.fontSize = 13;
        _blackFont.fontStyle = FontStyle.Normal;
        r = EditorGUILayout.GetControlRect(GUILayout.Width(42));
        r.x += 2;

        GUI.Label(r, new GUIContent(left), _blackFont);

        r.x += EditorGUILayout.GetControlRect().width - r.width - 2;

        GUI.Label(r, new GUIContent(right), _blackFont);

        GUILayout.Space(-40);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(40);
        float indent = 15;
        float size = 28;
        if (_useSkipToPrevious.boolValue) DrawIcon(4, size, indent);
        if (_useRewind.boolValue) DrawIcon(6, size, indent);
        DrawIcon(1, size, indent);
        if (_useFastForward.boolValue) DrawIcon(5, size, indent);
        if (_useSkipToNext.boolValue) DrawIcon(3, size, indent);
        if (_useStop.boolValue) DrawIcon(2, size, indent);
        if (_useClose.boolValue) DrawIcon(11, size, indent);
        GUILayout.Space(54);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(24);
        CUILayout.CenteredLabel(new GUIContent("Android 12 Preview"), 9);

        EditorGUILayout.EndVertical();
    }
}
