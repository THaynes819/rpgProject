using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{

    [SerializeField] QuestItemUI questPrefab;

    QuestList questList;

    // Start is called before the first frame update
    void Start ()
    {
        questList = GameObject.FindGameObjectWithTag ("Player").GetComponent<QuestList> ();
        questList.OnListUpdated += UpdateUI;
        UpdateUI ();
    }

    void UpdateUI ()
    {
        transform.DetachChildren ();
        foreach (Transform item in transform)
        {
            Destroy (item.gameObject);
        }
        foreach (QuestStatus status in questList.GetStatuses ())
        {
            QuestItemUI uiInstance = Instantiate<QuestItemUI> (questPrefab, transform);
            uiInstance.Setup (status);
        }

    }

}