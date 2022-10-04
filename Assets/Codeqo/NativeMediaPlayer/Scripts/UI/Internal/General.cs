using System;
using UnityEngine;

namespace Codeqo.NativeMediaPlayer.UI
{
    public enum ButtonType : short
    {
        PlayPauseToggle,
        PlayButton,
        PauseButton,
        StopButton,
        FastForwardButton,
        RewindButton,
        NextButton,
        PreviousButton,
        LoopOneAllToggle,
        LoopOneButton,
        LoopAllButton,
        ShuffleButton
    }

    public class ButtonColor
    {
        public class Enabled
        {
            public static Color LightMode;
            public static Color DarkMode;
        }

        public class Disabled
        {
            public static Color LightMode;
            public static Color DarkMode;
        }
    }

    public enum AddIndexType : int
    {
        [InspectorName("Don't add index")]
        None = 0,
        [InspectorName("Add index as prefix")]
        prefix = 1,
        [InspectorName("Add index as suffix")]
        suffix = 2
    }

    public class DefaultMediaMetadata
    {
        public static AddIndexType AddIndex = AddIndexType.None;
        public static string AlbumTitle = "Unknown Album Title";
        public static string AlbumArtist = "Unknown Album Artist";
        public static string Title = "Unknown Title";
        public static string Artist = "Unknown Artist";
    }

    public class Utilities
    { 
        public static Sprite ConvertBase64StringToSprite(string _data)
        {
            if (_data == null) return null;

            string encodedBase64String = _data;// PlayerPrefs.GetString(RETRIEVED_ART_DATA, defaultValue: "");
            if (encodedBase64String != "")
            {
                byte[] decoded = Convert.FromBase64String(encodedBase64String);
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(decoded);
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Debug.Log("Album art sprite successfully retrieved");
                return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            }
            else
            {
                return null;
            }
        }

        public static string ConvertSpriteToBase64String(Sprite _sprite)
        {
            if (_sprite == null) return null;

            try
            {
                Texture2D tex = _sprite.texture;
                var a = tex.EncodeToPNG();
                return Convert.ToBase64String(a);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }
    }
}
