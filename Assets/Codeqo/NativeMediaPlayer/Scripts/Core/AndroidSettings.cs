using UnityEngine;

public class AndroidSettings : MonoBehaviour
{    
    enum NotificationImportance : int
    {
        /* https://developer.android.com/reference/android/app/NotificationManager */
        IMPORTANCE_NONE = 0,
        IMPORTANCE_MIN = 1,
        IMPORTANCE_LOW = 2,
        IMPORTANCE_DEFAULT = 3,
        IMPORTANCE_HIGH = 4,
        IMPORTANCE_MAX = 5,
        IMPORTANCE_UNSPECIFIED = -1000
    }

    enum AudioUsage : int
    {
        /* https://developer.android.com/reference/android/media/AudioAttributes */
        USAGE_MEDIA = 1, // Usage value to use when the usage is media, such as music, or movie soundtracks.
        USAGE_GAME = 14, // Usage value to use when the usage is for game audio.
        USAGE_ALARM = 4, // Usage value to use when the usage is an alarm (e.g.wake-up alarm)
        USAGE_NOTIFICATION = 5, // Usage value to use when the usage is notification. See other notification usages for more specialized uses.
        USAGE_NOTIFICATION_EVENT = 10, // Usage value to use when the usage is to attract the user's attention, such as a reminder or low battery warning.
        USAGE_ASSISTANCE_ACCESSIBILITY = 11, // Usage value to use when the usage is for accessibility, such as with a screen reader.
        USAGE_ASSISTANCE_NAVIGATION_GUIDANCE = 12, // Usage value to use when the usage is driving or navigation directions.
        USAGE_ASSISTANCE_SONIFICATION = 13, // Usage value to use when the usage is sonification, such as with user interface sounds.
        USAGE_ASSISTANT = 16, // Usage value to use for audio responses to user queries, audio instructions or help utterances.
        USAGE_UNKNOWN = 0 // Usage value to use when the usage is unknown.
    }

    enum ContentType : int
    {
        /* https://developer.android.com/reference/android/media/AudioAttributes#CONTENT_TYPE_MOVIE */
        CONTENT_TYPE_MUSIC = 2, // Content type value to use when the content type is music.
        CONTENT_TYPE_MOVIE = 3, // Content type value to use when the content type is a soundtrack, typically accompanying a movie or TV program.
        CONTENT_TYPE_SONIFICATION = 4, // Content type value to use when the content type is a sound used to accompany a user action, such as a beep or sound effect expressing a key click, or event, such as the type of a sound for a bonus being received in a game. These sounds are mostly synthesized or short Foley sounds.
        CONTENT_TYPE_SPEECH = 1, // Content type value to use when the content type is speech.
        CONTENT_TYPE_UNKNOWN = 0
    }

    enum AllowedCapturePolicy : int 
    {
        /* Requires API29+ */
        /* https://developer.android.com/reference/android/media/AudioAttributes.Builder#setAllowedCapturePolicy(int) */
        /* https://developer.android.com/reference/android/media/AudioAttributes#ALLOW_CAPTURE_BY_ALL */
        ALLOW_CAPTURE_BY_ALL = 1, // Indicates that the audio may be captured by any app. For privacy, the following usages cannot be recorded: VOICE_COMMUNICATION*, USAGE_NOTIFICATION*, USAGE_ASSISTANCE* and USAGE_ASSISTANT. On Build.VERSION_CODES.Q, this means only USAGE_UNKNOWN, USAGE_MEDIA and USAGE_GAME may be captured.
        ALLOW_CAPTURE_BY_SYSTEM = 2, // Indicates that the audio may only be captured by system apps. System apps can capture for many purposes like accessibility, live captions, user guidance... but abide to the following restrictions: - the audio cannot leave the device - the audio cannot be passed to a third party app - the audio cannot be recorded at a higher quality than 16kHz 16bit mono
        ALLOW_CAPTURE_BY_NONE = 3 // Indicates that the audio is not to be recorded by any app, even if it is a system app. It is encouraged to use ALLOW_CAPTURE_BY_SYSTEM instead of this value as system apps provide significant and useful features for the user (such as live captioning and accessibility). 
    }

    enum AudioFocusChangeAction : int
    {
        [InspectorName("Pause")]
        ACTION_PLAYBACK_PAUSE = 1,
        [InspectorName("Resume")]
        ACTION_PLAYBACK_RESUME = 2,
        [InspectorName("Mute")]
        ACTION_VOLUME_MUTE = 3, // to 0
        [InspectorName("Lower Volume")]
        ACTION_VOLUME_QUIET = 4, // to .2
        [InspectorName("Raise Volume")]
        ACTION_VOLUME_MAX = 5, // to 1
        [InspectorName("None")]
        ACTION_NONE = 0
    }


    [SerializeField] protected Sprite smallIcon;
    [SerializeField] int notificationId = 0x342;
    [SerializeField] NotificationImportance notificationImportance = NotificationImportance.IMPORTANCE_LOW;
    [SerializeField] string notificationChannelId = "Default Channel Id";
    [SerializeField] protected string notificationChannelName = "Default Channel Name";
    [SerializeField] protected string notificationChannelDescription = "Default Channel Description";

    /* AudioAttributes */
    [SerializeField] AudioUsage audioUsage = AudioUsage.USAGE_GAME;
    [SerializeField] ContentType contentType = ContentType.CONTENT_TYPE_MUSIC;
    [SerializeField] AllowedCapturePolicy allowedCapturePolicy = AllowedCapturePolicy.ALLOW_CAPTURE_BY_ALL;
    /* AudioFocusRequest */
    /* https://developer.android.com/reference/android/media/AudioFocusRequest.Builder */
    [SerializeField] bool acceptsDelayedFocusGain = true; // Marks this focus request as compatible with delayed focus.
    [SerializeField] bool willPauseWhenDucked = false; // Declare the intended behavior of the application with regards to audio ducking.
    [SerializeField] AudioFocusChangeAction audioFocusGain = AudioFocusChangeAction.ACTION_PLAYBACK_RESUME;
    [SerializeField] AudioFocusChangeAction audioFocusGainTransient = AudioFocusChangeAction.ACTION_PLAYBACK_RESUME;
    [SerializeField] AudioFocusChangeAction audioFocusLoss = AudioFocusChangeAction.ACTION_PLAYBACK_PAUSE;
    [SerializeField] AudioFocusChangeAction audioFocusLossTransient = AudioFocusChangeAction.ACTION_PLAYBACK_PAUSE;
    [SerializeField] AudioFocusChangeAction audioFocusLossTransientCanDuck = AudioFocusChangeAction.ACTION_VOLUME_QUIET;

    [SerializeField] bool returnToAppOnNotificationClicked = false;
    [SerializeField] bool terminateAppOnNotificationDismissed = false;

    [SerializeField] bool useStop = false;
    [SerializeField] bool useSkipToNext = true;
    [SerializeField] bool useSkipToPrevious = true;
    [SerializeField] bool useFastForward = true;
    [SerializeField] bool useRewind = true;
    [SerializeField] bool useSkipToNextCompact = true;
    [SerializeField] bool useSkipToPreviousCompact = true;
    [SerializeField] bool useFastForwardCompact = false;
    [SerializeField] bool useRewindCompact = false;
    [SerializeField] bool useClose = false;

    public void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetInt("ANDROID_MEDIA_ACTION_STOP", useStop ? 1 : 0);
        PlayerPrefs.SetInt("ANDROID_MEDIA_ACTION_SKIP_TO_NEXT", useSkipToNext ? 1 : 0);
        PlayerPrefs.SetInt("ANDROID_MEDIA_ACTION_SKIP_TO_PREVIOUS", useSkipToPrevious ? 1 : 0);
        PlayerPrefs.SetInt("ANDROID_MEDIA_ACTION_FASTFORWARD", useFastForward ? 1 : 0);
        PlayerPrefs.SetInt("ANDROID_MEDIA_ACTION_REWIND", useRewind ? 1 : 0);
        PlayerPrefs.SetInt("ANDROID_MEDIA_ACTION_CLOSED", useClose ? 1 : 0);
        PlayerPrefs.SetInt("ANDROID_COMPACT_ACTION_SKIP_TO_NEXT", useSkipToNextCompact ? 1 : 0);
        PlayerPrefs.SetInt("ANDROID_COMPACT_ACTION_SKIP_TO_PREVIOUS", useSkipToPreviousCompact ? 1 : 0);
        PlayerPrefs.SetInt("ANDROID_COMPACT_ACTION_FASTFORWARD", useFastForwardCompact ? 1 : 0);
        PlayerPrefs.SetInt("ANDROID_COMPACT_ACTION_REWIND", useRewindCompact ? 1 : 0);

        PlayerPrefs.SetInt("ANDROID_RETURN_TO_APP", returnToAppOnNotificationClicked ? 1 : 0);
        PlayerPrefs.SetInt("ANDROID_TERMINATE_APP_ON_NOTIFICATION_DISMISSED", terminateAppOnNotificationDismissed ? 1 : 0);

        PlayerPrefs.SetInt("NOTIFICATION_ID", notificationId);
        PlayerPrefs.SetInt("NOTIFICATION_IMPORTANCE", (int)notificationImportance);
        PlayerPrefs.SetString("NOTIFICATION_CHANNEL_ID", notificationChannelId);
        PlayerPrefs.SetInt("AUDIO_ATTRIBUTES_USAGE", (int)audioUsage);
        PlayerPrefs.SetInt("AUDIO_ATTRIBUTES_CONTENT_TYPE", (int)contentType);
        PlayerPrefs.SetInt("AUDIO_ATTRIBUTES_ALLOWED_CAPTURE_POLICY", (int)allowedCapturePolicy);
        PlayerPrefs.SetInt("AUDIO_FOCUS_REQUEST_ACCEPTS_DELAYED_FOCUS_GAIN", acceptsDelayedFocusGain ? 1 : 0);
        PlayerPrefs.SetInt("AUDIO_FOCUS_REQUEST_WILL_PAUSE_WHEN_DUCKED", willPauseWhenDucked ? 1 : 0);
        PlayerPrefs.SetInt("AUDIO_FOCUS_CHANGE_ACTION_GAIN", (int)audioFocusGain);
        PlayerPrefs.SetInt("AUDIO_FOCUS_CHANGE_ACTION_GAIN_TRANSIENT", (int)audioFocusGainTransient);
        PlayerPrefs.SetInt("AUDIO_FOCUS_CHANGE_ACTION_LOSS", (int)audioFocusLoss);
        PlayerPrefs.SetInt("AUDIO_FOCUS_CHANGE_ACTION_LOSS_TRANSIENT", (int)audioFocusLossTransient);
        PlayerPrefs.SetInt("AUDIO_FOCUS_CHANGE_ACTION_LOSS_TRANSIENT_CAN_DUCK", (int)audioFocusLossTransientCanDuck);

        PlayerPrefs.Save();
    }
}
