using UnityEngine;

public class iOSSettings : MonoBehaviour
{
    /* Media Actions */
    [SerializeField] bool useStop = false;
    [SerializeField] bool useSkipToNext = true;
    [SerializeField] bool useSkipToPrevious = true;
    [SerializeField] bool useFastForward = true;
    [SerializeField] bool useRewind = true;
    [SerializeField] bool useSeekBar = true;


    public void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetInt("IOS_MEDIA_ACTION_STOP", useStop ? 1 : 0);
        PlayerPrefs.SetInt("IOS_MEDIA_ACTION_SKIP_TO_NEXT", useSkipToNext? 1 : 0);
        PlayerPrefs.SetInt("IOS_MEDIA_ACTION_SKIP_TO_PREVIOUS", useSkipToPrevious ? 1 : 0);
        PlayerPrefs.SetInt("IOS_MEDIA_ACTION_FASTFORWARD", useFastForward? 1 : 0); 
        PlayerPrefs.SetInt("IOS_MEDIA_ACTION_REWIND", useRewind ? 1 : 0);
        PlayerPrefs.SetInt("IOS_MEDIA_ACTION_SEEKBAR", useSeekBar? 1 : 0);
        PlayerPrefs.Save();
    }
}

