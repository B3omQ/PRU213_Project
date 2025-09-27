using UnityEngine;
using UnityEngine.EventSystems;

public class Dragitem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    Transform _originalParent;
    CanvasGroup _canvasGroup;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

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
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;

        Slot dropslot = eventData.pointerEnter?.GetComponent<Slot>();
        

        if (dropslot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropslot = dropItem.GetComponentInParent<Slot>();
            }
        }

        Slot originalSlot = _originalParent.GetComponent<Slot>();

        if (dropslot != null)
        {
            if (dropslot._currentItem != null)
            {
                dropslot._currentItem.transform.SetParent(originalSlot.transform);
                originalSlot._currentItem = dropslot._currentItem;
                dropslot._currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }else
            {
                originalSlot._currentItem = null;
            }

            transform.SetParent(dropslot.transform);
            dropslot._currentItem = gameObject;
        }else
        {
                transform.SetParent(_originalParent);
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero ;
    }

}
