using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Quest;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }
    public List<Quest.QuestProgress> activateQuests = new();
    private QuestUI questUI;

    public List<string> handinQuestIds = new();
private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        questUI =  FindObjectOfType<QuestUI>();
        InventoryController._instance._onInventoryChanged += CheckInventoryQuest;
    }

public void AcceptQuest(Quest quest)
    {
        if (IsQuestActive(quest._questId)) return;
        activateQuests.Add(new Quest.QuestProgress(quest));

        CheckInventoryQuest();
        questUI.UpdateQuestUI();
    }

public bool IsQuestActive(string questID) => activateQuests.Exists(q => q.QuestID == questID);

    public void CheckInventoryQuest()
    {
        Dictionary<int, int> itemCounts = InventoryController._instance._getItemCounts();

        foreach (Quest.QuestProgress quest in activateQuests)
        {
            foreach (Quest.QuestObjective questObjective in quest.objectives)
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

    public bool IsQuestCompleted(string questId)
    {
        QuestProgress quest = activateQuests.Find(q => q.QuestID == questId);
        return quest != null && quest.objectives.TrueForAll(o => o._isCompleted);
    }

    public void HandInQuest(string questId)
    {

        if (!RemoveRequiredItemsFromInventory(questId))
        {
            Debug.Log("not meet the item amount required to complete the quest");
            return;
        }
        QuestProgress quest = activateQuests.Find(q => q.QuestID == questId);
        if (quest != null)
        {
            handinQuestIds.Add(questId);
            activateQuests.Remove(quest);
            questUI.UpdateQuestUI();
            Debug.Log("Quest handed in and delete the quest");
        }
    }

    public bool isQuestHandedIn(string questId)
    {
        return handinQuestIds.Contains(questId);
    }

    public bool RemoveRequiredItemsFromInventory(string questId)
    {
        QuestProgress quest  = activateQuests.Find(q => q.QuestID == questId);
        if (quest == null) return false;
        Dictionary<int, int> requiredItems = new();
        //Item requirements from objectives
        foreach (QuestObjective objective in quest.objectives)
        {
            if (objective._type == ObjectiveType.CollectItem && int.TryParse(objective._questOpjectiveId, out int itemID))
            {
                requiredItems[itemID] = objective._requireAmount;
            }
        }
        //Verify we have items
        Dictionary<int, int> itemCounts = InventoryController._instance._getItemCounts();
        foreach (var item in requiredItems)
        {
            if (itemCounts.GetValueOrDefault(item.Key) < item.Value)
            {
                //Not enough items to complete quest
                return false;
            }
        }
        //Remove required items from inventory
        foreach (var itemRequirement in requiredItems)
        {
            //RemoveItemsFromInventory
            InventoryController._instance.RemoveItemsFromInventory(itemRequirement.Key, itemRequirement.Value);
        }
        return true;
    }

    public void LoadQuestProgress(List<Quest.QuestProgress> savedQuests)
    {
        activateQuests = savedQuests ?? new();

        CheckInventoryQuest();
        questUI.UpdateQuestUI();
    }
}
