using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;
using UnityEngine.UI;

public class QuestListUI : MonoBehaviour
{

    [SerializeField] QuestItemUI questPrefab;
    [SerializeField] Transform content = null;
    [SerializeField] Image animatedImage = null;
    [SerializeField] Animator questAnimator = null;

    QuestList questList;

    // Start is called before the first frame update
    void Start ()
    {
        questList = GameObject.FindGameObjectWithTag ("Player").GetComponent<QuestList> ();
        questList.OnListUpdated += UpdateUI;
        questList.OnAddQuest += AddQuest;
        questList.OnQuestUpdated += UpdateQuestObjective;
        questList.OnQuestCompleted += CompleteQuest;
        UpdateUI ();
        // Reset after updateUI to avoid Alerting on Start
        questAnimator.SetBool("giveQuest", false);
    }

    void UpdateUI ()
    {
        content.DetachChildren ();
        
        foreach (Transform item in content)
        {
            Destroy (item.gameObject);
        }
        foreach (QuestStatus status in questList.GetStatuses ())
        {
            QuestItemUI uiInstance = Instantiate<QuestItemUI> (questPrefab, content);
            uiInstance.Setup (status);
        }
        
    }

    private void AddQuest()
    {
        questAnimator.SetBool("giveQuest", true);
    }

    private void UpdateQuestObjective()
    {
        questAnimator.SetBool("updateQuest", true);
    }

    private void CompleteQuest()
    {
        questAnimator.SetBool("completeQuest", true);
    }

    //Resets the Quest alert to clear when Clicked
    public void AcknowledgeAlert()
    {
        questAnimator.SetBool("giveQuest", false);
        questAnimator.SetBool("updateQuest", false);
        questAnimator.SetBool("completeQuest", false);
        List<Quest> tempQuests = new List<Quest>();
        tempQuests = questList.GetRemovalQuests();
        foreach (var quest in tempQuests)
        {
            questList.AlertRemoveQuest(quest);
        }
        

        //TODO Add a "Complete Quest" Line where the recently completed Quest once was and make it fade away on Acknowledge
    }
}