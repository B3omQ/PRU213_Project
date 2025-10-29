using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int id;
    public string Name;
    public int quantity = 1;

    private TMP_Text quantityText;

    private Collider2D _collider;
    private bool canBePickedUp = false;

    private void Awake()
    {
        quantityText = GetComponentInChildren<TMP_Text>();
        _collider = GetComponent<Collider2D>();

        UpdateQuantityDisplay();
    }

    public void AddToStack(int amount = 1)
    {
        quantity += amount;
        UpdateQuantityDisplay();
    }

    public int RemoveFromStack(int amount = 1)
    {
        int removed = Mathf.Min(amount, quantity);
        quantity -= removed;
        UpdateQuantityDisplay();
        return removed;

    }

    public GameObject CloneItem(int newQuantity)
    {
        GameObject clone = Instantiate(gameObject);
        Item cloneItem = clone.GetComponent<Item>();
        cloneItem.quantity = newQuantity;
        cloneItem.UpdateQuantityDisplay();
        return clone;

    }

    public void UpdateQuantityDisplay()
    {
        if (quantityText != null)
        {
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
        }
    }
    public virtual void ShowPopUp()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;

        if (itemPickupUI.Instance != null)
        {
            itemPickupUI.Instance.ShowItemPickup(Name, itemIcon);
        }
    }

    public virtual void UseItem(Transform player)
    {
       
    }

    public virtual void OnEnable()
    {
        StartCoroutine(EnablePickupAfterDelay(2f));
    }

    public virtual IEnumerator EnablePickupAfterDelay(float delay)
    {
        canBePickedUp = false;
        yield return new WaitForSeconds(delay);
        canBePickedUp = true;
    }

    public virtual bool CanBePickedUp()
    {
        return canBePickedUp;
    }
}
