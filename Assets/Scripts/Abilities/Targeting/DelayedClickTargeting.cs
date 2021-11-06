using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu (fileName = "Delayed Click Targeting", menuName = "RPG/Abilities/Targeting/Delayed Click", order = 0)]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture;
        [SerializeField] Vector2 cursorHotspot;
        [SerializeField] float areaAffectRadius;
        [SerializeField] float range;
        [SerializeField] LayerMask layerMask;
        [SerializeField] Transform targetingPrefab;

        Transform targetingInstance = null;

        public override void StartTargeting (GameObject user, Action<IEnumerable<GameObject>> finished)
        {
            PlayerController playerController = user.GetComponent<PlayerController> ();
            playerController.StartCoroutine (Targeting (user, playerController, finished));
        }

        private IEnumerator Targeting (GameObject user, PlayerController playerController, Action<IEnumerable<GameObject>> finished)
        {
            playerController.enabled = false;
            if (targetingInstance == null)
            {
                targetingInstance = Instantiate (targetingPrefab);
            }
            else
            {
                targetingInstance.gameObject.SetActive (true);
            }
            targetingInstance.localScale = new Vector3(areaAffectRadius*2, 1, areaAffectRadius*2);
            while (true)
            {
                Cursor.SetCursor (cursorTexture, cursorHotspot, CursorMode.Auto);
                RaycastHit raycastHit;

                if (Physics.Raycast (PlayerController.GetMouseRay (), out raycastHit, range, layerMask))
                {
                    targetingInstance.position = raycastHit.point;

                    if (Input.GetMouseButtonDown (0))
                    {
                        // Nested While is to avoid moving to Cast Location - Absorb Whole MouseClick
                        while (Input.GetMouseButton (0))
                        {
                            yield return null;
                        }

                        playerController.enabled = true;
                        targetingInstance.gameObject.SetActive (false);
                        finished (GetGameObjectsInRadius (raycastHit.point));
                        yield break;
                    }
                    yield return null;
                }
            }

        }

        private IEnumerable<GameObject> GetGameObjectsInRadius (Vector3 point)
        {
            RaycastHit[] hits = Physics.SphereCastAll (point, areaAffectRadius, Vector3.up, 0);
            foreach (var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }
    }
}