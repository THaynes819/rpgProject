using System;
using System.Collections;
using Cinemachine;
using RPG.Pools;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace RPG.Control
{   
    public class Respawner : MonoBehaviour 
    {
        [SerializeField] Transform respawnLocation;
        [SerializeField] float respawnDelay = 3;
        [SerializeField] float fadeTime = 0.2f;
        [SerializeField] float healthRegenPercentage = 20;
        [SerializeField] float enemyHealthRegenPercentage = 20;


        Health health;
        

        private void Awake() 
        {
            health = GetComponent<Health>();
            health.onDeath.AddListener(Respawn);            
        }

        private void Start() 
        {
            if (health.IsDead())
            {
                Respawn();
            }
        }

        private void Respawn()
        {           
            StartCoroutine(RespawnRoutine());
        }

        IEnumerator RespawnRoutine()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            yield return new WaitForSeconds(respawnDelay);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeTime);
            RespawnPlayer();
            ResetEnemies();
            savingWrapper.Save();
            yield return fader.FadeIn(fadeTime);
        }


        private void RespawnPlayer()
        {
            Vector3 positionDelta = respawnLocation.position - transform.position;
            health.Heal(health.GetMaxHealthPoints() * healthRegenPercentage / 100, false, false,0, 0); 
            GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
            ICinemachineCamera activeVirtualCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            if (activeVirtualCamera.Follow == transform)
            {
                activeVirtualCamera.OnTargetObjectWarped(transform, positionDelta);
            }
        }

        private void ResetEnemies()
        {
            foreach (AIController enemyController in FindObjectsOfType<AIController>())
            {
                Health enemyHealth = enemyController.GetComponent<Health>();  
                
                if (enemyHealth && !enemyHealth.IsDead())
                {
                    enemyController.Reset();
                    enemyHealth.Heal(enemyHealth.GetMaxHealthPoints() * (enemyHealthRegenPercentage / 100), false, false, 0, 0);
                }
            }
        }
    }
}