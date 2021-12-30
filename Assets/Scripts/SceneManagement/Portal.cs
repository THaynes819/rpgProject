using System.Collections;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,
            B,
            C,
            D,
            E
        }

        [SerializeField] int sceneToLoad = 1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destinationPortal;
        [SerializeField] DestinationIdentifier currentPortal;
        [SerializeField] float portalFadeOutTime = 3f;
        [SerializeField] float portalFadeInTime = 1f;
        [SerializeField] float portalFadeWaitTime = 2f;

        NavMeshAgent navMeshAgent;

        private void Awake ()
        {

        }

        private void OnTriggerEnter (Collider other)
        {
            if (other.gameObject == GameObject.FindWithTag ("Player"))
            {
                StartCoroutine (Transition ());
            }
        }

        private IEnumerator Transition ()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError ("Scene to load not set.");
                yield break;
            }

            DontDestroyOnLoad (gameObject);

            Fader fader = FindObjectOfType<Fader> ();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper> ();
            PlayerController playerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();

            playerController.enabled = false;
            
            yield return fader.FadeOut (portalFadeOutTime);
            wrapper.Save ();
            
            yield return SceneManager.LoadSceneAsync (sceneToLoad);
            PlayerController newPlayerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
            newPlayerController.enabled = false;
            wrapper.Load ();
            
            Portal otherPortal = GetOtherPortal ();
            UpdatePlayer (otherPortal);
            
            wrapper.Save ();            
            yield return new WaitForSeconds (portalFadeWaitTime);
            
            fader.FadeIn (portalFadeInTime);
            newPlayerController.enabled = true;
            Destroy (gameObject);
        }

        private void UpdatePlayer (Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag ("Player");
            player.GetComponent<NavMeshAgent> ().Warp (otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal ()
        {
            foreach (Portal portal in FindObjectsOfType<Portal> ())
            {
                if (portal == this) continue;
                if (portal.currentPortal == destinationPortal)
                {
                    return portal;
                }
            }
            return null;
        }
    }
}