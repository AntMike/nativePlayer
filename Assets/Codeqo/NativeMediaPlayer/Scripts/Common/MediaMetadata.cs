using System.IO;
using Codeqo.NativeMediaPlayer.UI;
using UnityEngine;

namespace Codeqo.NativeMediaPlayer
{
    [System.Serializable]
    public class MediaMetadata
    {
        /* Android - MediaMetadata
         * https://developer.android.com/reference/android/media/MediaMetadata
         */
        /* iOS - MPMediaItem 
         * https://developer.apple.com/documentation/mediaplayer/mpmediaitem?language=objc
         */

        /* Android: METADATA_KEY_NUM_TRACKS */
        /* iOS: MPMediaItemPropertyAlbumTrackCount */
        // List<MediaMetadata>.Count

        /* Android: METADATA_KEY_ALBUM */
        /* iOS: MPMediaItemPropertyAlbumTitle */
        public string AlbumTitle;

        /* Android: METADATA_KEY_ALBUM_ARTIST */
        /* iOS: MPMediaItemPropertyAlbumArtist */
        public string AlbumArtist;

        /* Android: METADATA_KEY_TITLE */
        /* iOS: MPMediaItemPropertyTitle */
        public string Title;

        /* Android: METADATA_KEY_ART */
        /* iOS: MPMediaItemPropertyArtwork */
        public Sprite Artwork;
        public string ArtworkData;

        /* Android: METADATA_KEY_ARTIST */
        /* iOS: MPMediaItemPropertyArtist */
        public string Artist;

        /* Android: METADATA_KEY_GENRE */
        /* iOS: MPMediaItemPropertyGenre */
        public string Genre;

        /* Android: METADATA_KEY_DATE */
        /* iOS: MPMediaItemPropertyReleaseDate */
        public string ReleaseDate;

        /* Android: METADATA_KEY_DISPLAY_SUBTITLE */
        /* iOS: MPMediaItemPropertyLyrics */
        //public string Subtitle;

   

        public void Clear()
        {
            AlbumTitle = null;
            AlbumArtist = null;
            Title = null;
            Artist = null;
            Genre = null;
            ReleaseDate = null;
        }

        public void Save(int _id = -1)
        {
#if UNITY_ANDROID
            if (_id == -1) _id = MediaPlayer.CurrentMediaItemIndex;

            PlayerPrefs.SetString(_id + "_ALBUM_TITLE", AlbumTitle);
            PlayerPrefs.SetString(_id + "_ALBUM_ARTIST", AlbumArtist);
            PlayerPrefs.SetString(_id + "_TITLE", Title);
            PlayerPrefs.SetString(_id + "_ARTIST", Artist);
            PlayerPrefs.SetString(_id + "_GENRE", Genre);
            PlayerPrefs.SetString(_id + "_RELEASE_DATE", ReleaseDate);

            if (Artwork != null)
            {
                PlayerPrefs.SetString(_id + "_ART", Utilities.ConvertSpriteToBase64String(Artwork));
            }

            PlayerPrefs.Save();
#elif UNITY_IPHONE
            if (Artwork != null)
            {
                ArtworkData = Utilities.ConvertSpriteToBase64String(Artwork);
            }

            string path = Application.persistentDataPath + "/Track" + _id + ".json";
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(path, json, System.Text.Encoding.UTF8);
            Debug.Log("Track" + _id + " information saved to " + path);
#endif
        }

        public void Retrieve(int _id = -1)
        {
            Clear();

            Debug.Log("Retrieving Media Metadata #" + _id);
            
            if (_id == -1) _id = MediaPlayer.CurrentMediaItemIndex;            

            AlbumTitle = MediaPlayer.RetrieveAlbumTitle(_id);
            AlbumArtist = MediaPlayer.RetrieveAlbumArtist(_id);
            Title = MediaPlayer.RetrieveTitle(_id);
            Artist = MediaPlayer.RetrieveArtist(_id);
            Genre = MediaPlayer.RetrieveGenre(_id);
            ReleaseDate = MediaPlayer.RetrieveReleaseDate(_id);

            string temp = MediaPlayer.RetrieveArtwork(_id);
            if (temp != null)
            {
                Sprite tempSpr = Utilities.ConvertBase64StringToSprite(temp);
                if (temp != null) Artwork = tempSpr;
            }
        }
    }
}
