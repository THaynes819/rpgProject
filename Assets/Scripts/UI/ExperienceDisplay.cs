using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;


        GameObject player;
        BaseStats baseStats;


        private void Start ()
        {
            player = GameObject.FindWithTag ("Player");
            baseStats = player.GetComponent<BaseStats>();
        }

        void Update ()
        {
            foreground.localScale = new Vector3 (baseStats.GetExperienceFraction(), 1, 1);
        }
    }
}