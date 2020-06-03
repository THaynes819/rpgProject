using UnityEngine;

namespace RPG.UI.DamageText
{

	public class FloatingTextSpawner : MonoBehaviour
	{

		[SerializeField] DamageText damageTextPrefab = null;


		public void Spawn (float damage)
		{
			DamageText damageTextInstance = Instantiate<DamageText> (damageTextPrefab, transform);
		}
	}
}