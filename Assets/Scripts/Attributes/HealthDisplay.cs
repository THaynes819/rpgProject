using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{

    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        [SerializeField] bool isDisplayedAsPercent = true;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            if (isDisplayedAsPercent)
            {
                GetComponent<Text>().text = string.Format("{0:0}%", health.GetPercentage());
            }
            else
            {
                GetComponent<Text>().text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
            }
        }
    }
}