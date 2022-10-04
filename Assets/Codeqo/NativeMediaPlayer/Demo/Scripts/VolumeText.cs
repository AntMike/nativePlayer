using Codeqo.NativeMediaPlayer;
using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    public void UpdateText()
    {
        GetComponent<Text>().text = MediaPlayer.Volume.ToString("0.##") + "f";
    }
}
