using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;
        [SerializeField] TextMeshProUGUI currentXPEarned = null;
        [SerializeField] TextMeshProUGUI currentMaxXP = null;


        GameObject player;
        BaseStats baseStats;


        private void Start ()
        {
            player = GameObject.FindWithTag ("Player");
            baseStats = player.GetComponent<BaseStats>();
            
            currentXPEarned.text = baseStats.GetCurrentLevelXP().ToString("0");
            currentMaxXP.text = baseStats.GetXPToLevelUp().ToString("0");
        }

        void Update ()
        {
            foreground.localScale = new Vector3 (baseStats.GetExperienceFraction(), 1, 1);
            currentXPEarned.text = baseStats.GetCurrentLevelXP().ToString("0");
            currentMaxXP.text = baseStats.GetXPToLevelUp().ToString("0");
        }
    }
}