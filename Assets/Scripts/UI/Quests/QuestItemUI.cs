using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI progress;

    QuestStatus status;


    public void Setup(QuestStatus status)
    {
        this.status = status;
        title.text = status.GetQuest().GetQuesttitle();
        progress.text = status.GetCompletedCount() + "/" + status.GetQuest().GetObjectiveCount();
    }

    public QuestStatus GetQuestStatus()
    {
        return status;
    }
}
