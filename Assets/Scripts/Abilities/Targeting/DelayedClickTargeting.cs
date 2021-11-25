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

        public override void StartTargeting (AbilityData data, Action finished)
        {
            PlayerController playerController = data.GetUser().GetComponent<PlayerController> ();
            playerController.StartCoroutine (Targeting (data, playerController, finished));
        }

        private IEnumerator Targeting (AbilityData data, PlayerController playerController, Action finished)
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
            while (!data.GetIsCancelled())
            {
                Cursor.SetCursor (cursorTexture, cursorHotspot, CursorMode.Auto);
                RaycastHit raycastHit;

                if (Physics.Raycast (PlayerController.GetMouseRay (), out raycastHit, range, layerMask))
                {
                    targetingInstance.position = raycastHit.point;

                    if (Input.GetMouseButtonDown (0))
                    {
                        // absorb the whole mouse click
                        yield return new WaitWhile(() => Input.GetMouseButton(0));
                        
                        
                        data.SetTargetedPoint(raycastHit.point);
                        data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                        
                        break;
                    }
                    if (Input.GetMouseButton(1))
                    {
                        data.Cancel();
                        break;
                    }                    
                    yield return null;
                }  
            }
            targetingInstance.gameObject.SetActive (false);
            playerController.enabled = true;
            finished ();   
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