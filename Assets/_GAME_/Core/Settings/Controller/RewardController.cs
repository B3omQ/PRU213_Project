using UnityEngine;

public class RewardController : MonoBehaviour
{
   public static RewardController Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GiveQuestReward(Quest quest)
    {
        if(quest?._questReward == null) return;

        foreach (var reward in quest._questReward)
        {
            switch (reward._type)
            {
                case Quest.RewardType.Item:
                    GiveItemReward(reward._rewardId, reward._amount);
                    break;
                case Quest.RewardType.Gold:
                    break;
                case Quest.RewardType.Exp:
                    break;
                case Quest.RewardType.Custom:
                    break;
            }
        }
    }

    public void GiveItemReward(int itemId, int amount)
    {
        var itemPrefab = FindAnyObjectByType<ItemDictionary>()?.GetItemPrefab(itemId);

        if (itemPrefab == null) return;

        for (int i = 0; i < amount; i++)
        {
            if (!InventoryController._instance.AddItem(itemPrefab))
            {
                GameObject dropItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
                dropItem.GetComponent<BounceEffect>().StartBounce();
            }
            else
            {
                //showPopUp 
                itemPrefab.GetComponent<Item>().ShowPopUp();
            }
        }
    }
}
