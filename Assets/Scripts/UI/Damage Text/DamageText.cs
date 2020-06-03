using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text damageText = null;

        public void DestroyText ()
        {
            Destroy (gameObject);
        }

        public void SetValue (float amount)
        {
            damageText.text = string.Format ("{0:0}", amount);
            //string.Format ("{0:0}/{1:0}", amount);

        }

    }
}