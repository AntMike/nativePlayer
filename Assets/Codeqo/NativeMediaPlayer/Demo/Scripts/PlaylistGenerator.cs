using Codeqo.NativeMediaPlayer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaylistGenerator : MonoBehaviour
{
    [SerializeField] GameObject loadingPrefab, folderPrefab, albumInfoPrefab, mediaItemPrefab;
    [SerializeField] Transform parentTransform;
    List<GameObject> playlistFolder = new List<GameObject>();
    List<GameObject> allObjects = new List<GameObject>();
    UnityAction onInitAction;

    void Awake()
    {
        onInitAction = () =>
        {
            GetComponent<Slide>().transforms = new List<Transform>();

            for (int i = 0; i < MediaController.Count; i++)
            {
                /* Parent Folder */
                GameObject temp = new GameObject("Playlist#" + i);
                GameObject list = Instantiate(temp, parentTransform);
                allObjects.Add(list);
                Destroy(temp);
                
                RectTransform rect = list.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 1);
                int addPos = 1440 * i;
                list.transform.localPosition = new Vector2(list.transform.localPosition.x + addPos, list.transform.localPosition.y);
                GetComponent<Slide>().transforms.Add(list.transform);

                /* List Folder */                
                GameObject folder = Instantiate(folderPrefab, list.transform);
                GameObject header = Instantiate(albumInfoPrefab, folder.transform);
                allObjects.Add(folder);
                allObjects.Add(header);
                header.GetComponent<PlaylistUI>().Create(i);
                CreateMediaItems(folder.transform, i);
                playlistFolder.Add(folder);                

                /* Loading */                
                GameObject loading = Instantiate(loadingPrefab, list.transform);
                allObjects.Add(loading);
                PlaylistLoading.Attach(list, i, loading.GetComponent<CanvasGroup>(), loading.transform.GetChild(0).GetComponent<RectTransform>());                
            }
        };

        MediaEvents.OnInit.AddListener(onInitAction);
    }

    void OnDestroy()
    {
        DestroyPlaylists();
        MediaEvents.OnInit.RemoveListener(onInitAction);
    }

    void DestroyPlaylists()
    {
        for (int i = 0; i < allObjects.Count; i++)
        {
            Destroy(allObjects[i]);
        }
        allObjects = null;
    }

    public void DeleteFolder(int _id)
    {
        playlistFolder.RemoveAt(_id);
    }

    public void CreateMediaItems(Transform parent, int playlistId)
    {
        for (int i = 0; i < MediaController.Playlists[playlistId].MediaItems.Count; i++)
        {
            CreateMediaItem(parent, playlistId, i);
        }
    }

    public void CreateMediaItem(Transform parent, int playlistId, int mediaItemId)
    {
        GameObject go = Instantiate(mediaItemPrefab);
        allObjects.Add(go);
        go.transform.SetParent(parent);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.GetComponent<MediaItemUI>().Create(playlistId, mediaItemId);
        go.GetComponent<MediaItemUI>().selectEvent.AddListener(() => { 
            transform.parent.GetComponent<Slide>().SlideToLeft();
        });
    }
}
