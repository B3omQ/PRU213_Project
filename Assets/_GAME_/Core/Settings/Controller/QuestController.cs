using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Guest;

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
        InventoryController._instance._onInventoryChanged += CheckInventoryQuest;
    }

public void AcceptQuest(Guest quest)
    {
        if (IsQuestActive(quest._questId)) return;
        activateQuests.Add(new Guest.QuestProgress(quest));

        CheckInventoryQuest();
        questUI.UpdateQuestUI();
    }

public bool IsQuestActive(string questID) => activateQuests.Exists(q => q.QuestID == questID);

    public void CheckInventoryQuest()
    {
        Dictionary<int, int> itemCounts = InventoryController._instance._getItemCounts();

        foreach (Guest.QuestProgress quest in activateQuests)
        {
            foreach (Guest.QuestObjective questObjective in quest.objectives)
            {
                if (questObjective._type != ObjectiveType.CollectItem) continue;
                if (!int.TryParse(questObjective._questOpjectiveId, out int itemId)) continue;

                int newAmount = itemCounts.TryGetValue(itemId, out int count) ? Mathf.Min(count, questObjective._requireAmount) : 0;

                if (questObjective._currentAmount != newAmount)
                {
                    questObjective._currentAmount = newAmount;
                }
            }
            
        }
        questUI.UpdateQuestUI();
    }

    public void LoadQuestProgress(List<Guest.QuestProgress> savedQuests)
    {
        activateQuests = savedQuests ?? new();

        CheckInventoryQuest();
        questUI.UpdateQuestUI();
    }
}
