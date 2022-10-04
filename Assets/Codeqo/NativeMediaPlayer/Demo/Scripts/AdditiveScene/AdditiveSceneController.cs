using UnityEngine;

public class AdditiveSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        AdditiveSceneManager.isLoader = true;
        AdditiveSceneManager.isLoaded = true;
    }

}
