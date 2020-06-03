using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {

        [SerializeField] RectTransform foreground = null;
        [SerializeField] Health healthComponent = null;

        float healthFraction;
        float foregroundXScale;



        void Update ()
        {
            if (healthComponent != healthComponent.IsDead())
            {
            foreground.localScale = new Vector3 (healthComponent.GetFraction(), 1, 1);
            }
            else
            {
                Destroy(this.gameObject);
            }

        }

    }

}