using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragitem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{

    Transform _originalParent;
    CanvasGroup _canvasGroup;

    public float _minDropdis = 1f;
    public float _maxDropdis = 2f;

    private InventoryController _inventoryController;
    
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _inventoryController = InventoryController._instance;

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalParent = transform.parent;
        transform.SetParent(transform.root);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true; //Enables raycasts
        _canvasGroup.alpha = 1f; //No longer transparent

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>(); //Slot where item dropped
        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }
        Slot originalSlot = _originalParent.GetComponent<Slot>();

        if (dropSlot == originalSlot)
        {
            transform.SetParent(_originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            return;
        }

        if (dropSlot != null)
        {
            //Is a slot under drop point
            if (dropSlot._currentItem != null)
            {
                Item draggedItem = GetComponent<Item>();
                Item targetItem = dropSlot._currentItem.GetComponent<Item>();

                if (draggedItem.id == targetItem.id)
                {
                    targetItem.AddToStack(draggedItem.quantity);
                    originalSlot._currentItem = null;
                    Destroy(gameObject);
                }
                else
                {
                    //Slot has an item - swap items
                    dropSlot._currentItem.transform.SetParent(originalSlot.transform);
                    originalSlot._currentItem = dropSlot._currentItem;
                    dropSlot._currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    transform.SetParent(dropSlot.transform);
                    dropSlot._currentItem = gameObject;
                    GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //Center
                }
            }
            else
            {
                originalSlot._currentItem = null;
                transform.SetParent(dropSlot.transform);
                dropSlot._currentItem = gameObject;
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //Center
            }
        }
        else
        {
            //No slot under drop point
            //If where we're dropping is not within the inventory
            if (!IsWithinInventory(eventData.position))
            {
                //Drop our item
                DropItem(originalSlot);
            }
            else
            {
                //Snap back to og slot
                transform.SetParent(_originalParent);
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //Center
            }
        }
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect =  _originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition); 
    }

    void DropItem(Slot originalSlot)
    {
        Item item = GetComponent<Item>();
        int quantity = item.quantity;

        if (quantity <= 0) return;

        originalSlot._currentItem = null;

        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Missing 'Player' tag");
            return;
        }

        // Lặp theo số lượng item
        for (int i = 0; i < quantity; i++)
        {
            // Random vị trí xung quanh player
            Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(_minDropdis, _maxDropdis);
            Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

            // Tạo bản sao item
            GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity);

            // Cấu hình lại item
            Item droppedItem = dropItem.GetComponent<Item>();
            droppedItem.quantity = 1;

            // Chạy hiệu ứng bounce nếu có
            BounceEffect bounce = dropItem.GetComponent<BounceEffect>();
            if (bounce != null)
                bounce.StartBounce();
        }

        // Xoá item trong inventory sau khi thả hết ra ngoài
        Destroy(gameObject);

        InventoryController._instance.RebuidItemCounts();

        Item itemComponent = GetComponent<Item>();
        itemComponent?.OnDropOutsideInventory();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            SlpitStack();
        }
    }

    private void SlpitStack()
    {
        Item item = GetComponent<Item>();
        if(item == null || item.quantity <= 1) return;
        int splitAmount = item.quantity / 2;
        item.RemoveFromStack(splitAmount);

        GameObject newItem = item.CloneItem(splitAmount);

        if (_inventoryController == null || newItem == null) return;

        foreach (Transform slotTransform in _inventoryController._inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot._currentItem == null)
            {
                slot._currentItem = newItem;
                newItem.transform.SetParent(slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
        }

        item.AddToStack(splitAmount);
        Destroy(newItem);
    }
}
