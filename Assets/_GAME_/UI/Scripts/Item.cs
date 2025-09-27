using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int id;
    public string Name;

    public virtual void PickUp()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;

        if (itemPickupUI.Instance != null)
        {
            itemPickupUI.Instance.ShowItemPickup(Name, itemIcon);
        }
    }
}
