using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using static Quest;

public class QuestUI : MonoBehaviour
{
    public Transform _questListContent;
    public GameObject _questEntryPrefab;
    public GameObject _objectiveTextPrefab;

    //public Guest _textQuest;
    //public int _testQuestAmount;
    //private List<QuestProgress> _testQuests = new();
    void Start()
    {
        //for (int i = 0; i < _testQuestAmount; i ++)
        //{
        //    _testQuests.Add(new QuestProgress(_textQuest));
        //}
        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        foreach (Transform child in _questListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var quest in QuestController.Instance.activateQuests)
        {
            GameObject entry = Instantiate(_questEntryPrefab, _questListContent);
            TMP_Text questNameText = entry.transform.Find("QuestName").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectiveList");

            questNameText.text = quest.quest.name;

            foreach (var objective in quest.objectives)
            {
                GameObject objTextGO = Instantiate(_objectiveTextPrefab, objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective._questOpjectiveDes} ({objective._currentAmount} / {objective._requireAmount})";
            }
        }
    }
}
