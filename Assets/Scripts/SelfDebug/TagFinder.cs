using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SelfDebug
{
        public class TagFinder : MonoBehaviour
    {
        [SerializeField] string tagToSearch = "Player";

        [SerializeField] KeyCode searchKey = KeyCode.Return;
        [SerializeField] GameObject[] alertPrefabs = null;

        GameObject[] searchObjects;

        // Start is called before the first frame update
        void Start()
        {
            searchObjects = GameObject.FindGameObjectsWithTag (tagToSearch);
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(searchKey))
            {
                foreach (GameObject searchObject in searchObjects)
                {
                    foreach (GameObject prefab in alertPrefabs)
                    {
                        Instantiate(prefab, searchObject.transform);
                        Debug.Log("Search Result: " + searchObject.name);
                    }
                }
            }
        }
    }

}