using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI progress;


    public void Setup(Quest quest)
    {
        title.text = quest.GetQuesttitle();
        progress.text = "1" + "/" + quest.GetObjectives().GetLength(1).ToString();
    }
}
