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
        Debug.Log("Setting Initial Quest to false");
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
        Debug.Log("Giving Quest, Alerting UI");
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
    public void AcknowledgeQuest()
    {
        Debug.Log("Acknowledging Quest Alert");
        questAnimator.SetBool("giveQuest", false);
        questAnimator.SetBool("updateQuest", false);
        questAnimator.SetBool("completeQuest", false);
        // var tempColor = animatedImage.color;
        // tempColor.a = 0f;
        // animatedImage.color = tempColor;
        // Debug.Log("animated Image alpha is " + animatedImage.color.a);
    }
}