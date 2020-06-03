using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {

        [SerializeField] RectTransform foreground = null;
        [SerializeField] Health healthComponent = null;
        [SerializeField] Canvas rootCanvas = null;

        float healthFraction;
        float foregroundXScale;

        private void Start ()
        {
            rootCanvas.enabled = false;
        }

        void Update ()
        {
            if (Mathf.Approximately (healthComponent.GetFraction (), 1) || healthComponent.IsDead())
            {
                rootCanvas.enabled = false;
                return;
            }
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3 (healthComponent.GetFraction (), 1, 1);


        }

    }

}