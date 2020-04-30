using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}