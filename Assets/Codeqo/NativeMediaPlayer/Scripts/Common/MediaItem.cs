using UnityEngine;
using UnityEngine.Events;

namespace Codeqo.NativeMediaPlayer
{

    [System.Serializable]
    public class MediaItem
    {
        [SerializeField] RetrieveMetadataType MetadataType = RetrieveMetadataType.RetrieveMediaMetadata;
        [SerializeField] RetrieveArtworkType ArtworkType = RetrieveArtworkType.RetrieveArtwork;
        [SerializeField] Object StreamingAsset;
        [SerializeField] MediaMetadata MediaMetadata;
        private UnityAction RetrieveData;

        public int Id = -1;
        public UriType Path;
        public string MediaUri;

        public bool HasCustomMediaMetadata => MetadataType == RetrieveMetadataType.AddCustomMediaMetadata;

        public string Title => MediaMetadata.Title;
        public string Artist => MediaMetadata.Artist;
        public string Genre => MediaMetadata.Genre;
        public string ReleaseDate => MediaMetadata.ReleaseDate;
        public string AlbumTitle => MediaMetadata.AlbumTitle;
        public string AlbumArtist => MediaMetadata.AlbumArtist;
        public Sprite Artwork => MediaMetadata.Artwork;


        public MediaItem(UriType type, string uri, MediaMetadata metadata = null)
        {
            if (metadata == null) metadata = new MediaMetadata();
            MediaUri = uri;
            Path = type;
            MediaMetadata = metadata;
        }

        public void Sync()
        {            
            if (RetrieveData != null) MediaEvents.OnReady.RemoveListener(RetrieveData);

            PlayerPrefs.SetInt(Id + "_TYPE_ARTWORK", (short)ArtworkType);
            PlayerPrefs.SetInt(Id + "_TYPE_METADATA", (short)MetadataType);
            PlayerPrefs.SetString(Id + "_MEDIA_URI", MediaUri);
            PlayerPrefs.Save();

            if (MetadataType == RetrieveMetadataType.AddCustomMediaMetadata)
            {
                MediaMetadata.Save(Id);
            }
            else
            {
                MediaEvents.MetadataUpdate.Add(Id, new UnityAction(() => MediaMetadata.Retrieve(Id)));
            }
        }
    }
}
