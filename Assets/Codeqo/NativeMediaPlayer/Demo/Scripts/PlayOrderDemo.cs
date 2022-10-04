using Codeqo.NativeMediaPlayer;
using UnityEngine;
using UnityEngine.UI;

public class PlayOrderDemo : MonoBehaviour
{
    void Awake()
    {
        MediaEvents.OnPrepared.AddListener(UpdatePlayOrder); 
    }

    void OnDestroy()
    {
        MediaEvents.OnPrepared.RemoveListener(UpdatePlayOrder);
    }

    public void UpdatePlayOrder()
    {
        int[] order = MediaPlayer.GetShuffleOrder();

        if (order == null)
        {
            Debug.LogError("shuffleOrder doesn't exist");
            return;
        }

        if (order.Length > 1)
        {
            string shuffle = "";
            for (int i = 0; i < order.Length; i++)
            {
                if (MediaPlayer.CurrentMediaItemIndex == order[i])
                    shuffle += "<color=#3484F0>" + (order[i] + 1) + "</color>, ";
                else
                    shuffle += (order[i] + 1) + ", ";
            }
            shuffle = shuffle.Substring(0, shuffle.Length - 2);
            GetComponent<Text>().text = shuffle;
        }
        else
        {
            GetComponent<Text>().text = "None";
        }
    }
}
