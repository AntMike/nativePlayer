using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CodeqoEditor
{
    public class CUILayoutMediaUtility
    {
        /* Media Icon Paths */
        private const string _ic_play = "Assets/Codeqo/GUISkins/Icons/ic_play_arrow_black_36dp.png";
        private const string _ic_stop = "Assets/Codeqo/GUISkins/Icons/ic_stop_black_36dp.png";
        private const string _ic_fast_forward = "Assets/Codeqo/GUISkins/Icons/ic_fast_forward_black_36dp.png";
        private const string _ic_fast_rewind = "Assets/Codeqo/GUISkins/Icons/ic_fast_rewind_black_36dp.png";
        private const string _ic_skip_next = "Assets/Codeqo/GUISkins/Icons/ic_skip_next_black_36dp.png";
        private const string _ic_skip_previous = "Assets/Codeqo/GUISkins/Icons/ic_skip_previous_black_36dp.png";
        private const string _ic_repeat_all = "Assets/Codeqo/GUISkins/Icons/baseline_repeat_black_36.png";
        private const string _ic_shuffle = "Assets/Codeqo/GUISkins/Icons/baseline_shuffle_black_36.png";
        private const string _ic_volumebar = "Assets/Codeqo/GUISkins/Icons/ic_volumebar.png";
        private const string _ic_seekbar = "Assets/Codeqo/GUISkins/Icons/ic_seekbar.png";
        private const string _ic_close = "Assets/Codeqo/GUISkins/Icons/baseline_close_black_36.png";

        private static string[] MediaActions =
        {
                "PLAYPAUSE",
                "STOP",
                "NEXT",
                "PREVIOUS",
                "FASTFORWARD",
                "REWIND",
                "LOOP",
                "SHUFFLE",
                "CLOSE"
        };

        /* Public Consts */
        public const int PlayPause = 1;
        public const int Stop = 2;
        public const int Next = 3;
        public const int Previous = 4;
        public const int FastForward = 5;
        public const int Rewind = 6;
        public const int Loop = 7;
        public const int Shuffle = 8;
        public const int Volume = 9;
        public const int Seek = 10;
        public const int Close = 11;

        public static Texture2D GetIconTexture(int value)
        {
            switch (value)
            {
                case 1: return (Texture2D)AssetDatabase.LoadAssetAtPath(_ic_play, typeof(Texture2D));
                case 2: return (Texture2D)AssetDatabase.LoadAssetAtPath(_ic_stop, typeof(Texture2D));
                case 3: return (Texture2D)AssetDatabase.LoadAssetAtPath(_ic_skip_next, typeof(Texture2D));
                case 4: return (Texture2D)AssetDatabase.LoadAssetAtPath(_ic_skip_previous, typeof(Texture2D));
                case 5: return (Texture2D)AssetDatabase.LoadAssetAtPath(_ic_fast_forward, typeof(Texture2D));
                case 6: return (Texture2D)AssetDatabase.LoadAssetAtPath(_ic_fast_rewind, typeof(Texture2D));
                case 7: return (Texture2D)AssetDatabase.LoadAssetAtPath(_ic_repeat_all, typeof(Texture2D));
                case 8: return (Texture2D)AssetDatabase.LoadAssetAtPath(_ic_shuffle, typeof(Texture2D));
                case 9: return (Texture2D)AssetDatabase.LoadAssetAtPath(_ic_volumebar, typeof(Texture2D));
                case 10: return (Texture2D)AssetDatabase.LoadAssetAtPath(_ic_seekbar, typeof(Texture2D));
                case 11: return (Texture2D)AssetDatabase.LoadAssetAtPath(_ic_close, typeof(Texture2D));
                default: return null;
            }
        }

        public static int GetMediaActionIndex(string action)
        {
            string filtered = action.ToUpper().Replace("/", "").Replace("-", "").Replace(" ", "");
            switch (MediaActions.FirstOrDefault<string>(s => filtered.Contains(s)))
            {
                case "PLAYPAUSE": return 1;
                case "STOP": return 2;
                case "NEXT": return 3;
                case "PREVIOUS": return 4;
                case "FASTFORWARD": return 5;
                case "REWIND": return 6;
                case "LOOP": return 7;
                case "SHUFFLE": return 8;
                case "CLOSE": return 11;
                default: Debug.Log("Invalid string value : " + filtered); return 0;
            }
        }
    }
}

