using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {

        [SerializeField] int sceneToLoad = 1;
        
        GameObject player;


        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject == player)
            {            
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {            
            DontDestroyOnLoad(gameObject);           
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            print("Scene Loaded");
            Destroy(gameObject);            
        }
    }
}