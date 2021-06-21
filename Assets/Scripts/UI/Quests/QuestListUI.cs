﻿using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] Quest[] tempQuests;
    [SerializeField] QuestItemUI questPrefab;

    // Start is called before the first frame update
    void Start ()
    {
        foreach (Quest quest in tempQuests)
        {
            Instantiate (questPrefab);
        }
    }

}