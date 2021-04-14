using RPG.Combat;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGlobe : MonoBehaviour
{

    [SerializeField] Slider slider = null;

    GameObject player;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
    }

    void Update ()
    {
        slider.value = player.GetComponent<ResourcePool> ().GetPoolFraction ();
    }
}