using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static Quest;


[CreateAssetMenu(menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string _questId;
    public string _questName;
    public string _description;
    public List<QuestObjective> _objectives;
    public List<QuestReward> _questReward;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(_questId))
        {
            _questId = _questName + Guid.NewGuid().ToString();
        }
    }

    [System.Serializable]
    public class QuestObjective
    {
        public string _questOpjectiveId;
        public string _questOpjectiveDes;
        public ObjectiveType _type;
        public int _requireAmount;
        public int _currentAmount;

        public bool _isCompleted => _currentAmount >= _requireAmount;
    }

    public enum ObjectiveType
    {
        CollectItem, DefeatEnemy, ReachLocation, TalkNPC, Custom
    }

    [System.Serializable]
    public class QuestProgress
    {
        public Quest quest;
        public List<QuestObjective> objectives;
        public QuestProgress(Quest quest)
        {
            this.quest = quest;
            objectives  = new List<QuestObjective>();
            //Deep copy avoid modifying original
            foreach (var obj in quest._objectives)
            {
                objectives.Add(new QuestObjective
                {
                    _questOpjectiveId = obj._questOpjectiveId,
                    _questOpjectiveDes = obj._questOpjectiveDes,
                    _type = obj._type,
                    _requireAmount = obj._requireAmount,
                    _currentAmount = 0
                });
            }
        }
        public bool IsCompleted => objectives.TrueForAll(o => o._isCompleted);
        public string QuestID => quest._questId;
    }

    [System.Serializable]
    public class QuestReward
    {
        public RewardType _type;
        public int _rewardId;
        public int _amount = 1;
    }

    public enum RewardType
    {
        Item,
        Gold,
        Exp,
        Custom
    }
}
