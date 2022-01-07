using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Pools
{
    public class HealthGlobeUI : MonoBehaviour
    {

        [SerializeField] Slider slider = null;
        [SerializeField] TextMeshProUGUI currentHealth;
        [SerializeField] TextMeshProUGUI maxHealth;

        GameObject player;

        void Start ()
        {
            player = GameObject.FindGameObjectWithTag ("Player");
        }

        void Update ()
        {
            slider.value = player.GetComponent<Health> ().GetFraction ();
            currentHealth.text = player.GetComponent<Health>().GetHealthPoints().ToString("F0");
            maxHealth.text = player.GetComponent<Health>().GetMaxHealthPoints().ToString("F0");
        }
    }
}