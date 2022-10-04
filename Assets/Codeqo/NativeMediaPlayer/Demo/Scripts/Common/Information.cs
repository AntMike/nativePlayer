using UnityEngine;
using UnityEngine.UI;

public class Information : MonoBehaviour
{
    [SerializeField] Sprite icon;
    [SerializeField] string type;
    [SerializeField] string credit;
    [SerializeField] string lastUpdateDate;

    void Start()
    {
        transform.GetChild(0).GetComponent<Text>().text = type;
        transform.GetChild(1).GetComponent<Text>().text = Application.productName;
        transform.GetChild(2).GetComponent<Text>().text = "Version " + Application.version + " (" + lastUpdateDate + ")";
    }
}
