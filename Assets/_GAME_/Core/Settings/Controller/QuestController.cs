using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }
    public List<Guest.QuestProgress> activateQuests = new();
    private QuestUI questUI;
private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        questUI =  FindObjectOfType<QuestUI>();
    }

public void AcceptQuest(Guest quest)
    {
        if (IsQuestActive(quest._questId)) return;
        activateQuests.Add(new Guest.QuestProgress(quest));
        questUI.UpdateQuestUI();
    }

public bool IsQuestActive(string questID) => activateQuests.Exists(q => q.QuestID == questID);

}
