using RPG.Pools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGlobeUI : MonoBehaviour
{

    [SerializeField] Slider slider = null;
    [SerializeField] TextMeshProUGUI currentResource;
    [SerializeField] TextMeshProUGUI maxResource;

    GameObject player;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
    }

    void Update ()
    {
        slider.value = player.GetComponent<ResourcePool> ().GetPoolFraction ();
        currentResource.text = player.GetComponent<ResourcePool>().GetCurrentResourcePoints().ToString("0");
        maxResource.text = player.GetComponent<ResourcePool>().GetCurrentMaxPool().ToString("0");
    }
}