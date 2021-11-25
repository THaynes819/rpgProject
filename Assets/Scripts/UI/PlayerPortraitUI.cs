using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using TMPro;
using UnityEngine;

public class PlayerPortraitUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerLevelText = null;

    GameObject player;
    BaseStats baseStats;

    void Awake() 
    {
        player = GameObject.FindWithTag ("Player");
        baseStats = player.GetComponent<BaseStats>();
        baseStats.onLevelUp += UpdateLevelDisplay;
    }
    void Start()
    {    
        playerLevelText.text = baseStats.GetLevel().ToString();
    }

        public void UpdateLevelDisplay()
        {
            playerLevelText.text = baseStats.GetLevel().ToString();
        }
}
