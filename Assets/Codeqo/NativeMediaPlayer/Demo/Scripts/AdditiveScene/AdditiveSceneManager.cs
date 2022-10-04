using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneManager : MonoBehaviour
{
    const string SceneName = "SampleScene";
    public static bool isLoader = false;
    public static bool isLoaded;

    void Start()
    {
        if (!isLoaded) Destroy(this.gameObject);
        if (!isLoader) Destroy(GameObject.Find("EventSystem"));
    }

    public void LoadScene()
    {
        isLoaded = true;
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        Debug.Log("Load Scene");
    }

    public void UnloadScene()
    {
        isLoaded = false;
        SceneManager.UnloadSceneAsync(SceneName);
        Debug.Log("Unload Scene");
    }
}
