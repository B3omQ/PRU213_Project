using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool _isOpened { get; private set; }
    public string _chestId { get; private set; }

    [Header("Chest Settings")]
    public List<GameObject> _itemPrefabs; // Danh sách item
    public Sprite _openedChest;
    public float _dropSpacing = 0.5f; // Khoảng cách giữa các item rơi

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    public bool CanInteract()
    {
        return !_isOpened;
    }

    private void OpenChest()
    {
        SetOpened(true);

        if (_itemPrefabs != null && _itemPrefabs.Count > 0)
        {
            for (int i = 0; i < _itemPrefabs.Count; i++)
            {
                Vector3 dropPos = transform.position + Vector3.down + new Vector3(i * _dropSpacing, 0, 0);
                GameObject droppedItem = Instantiate(_itemPrefabs[i], dropPos, Quaternion.identity);
                droppedItem.GetComponent<BounceEffect>()?.StartBounce();
            }
        }
    }

    public void SetOpened(bool opened)
    {
        _isOpened = opened;
        if (_isOpened)
        {
            GetComponent<SpriteRenderer>().sprite = _openedChest;
        }
    }
}
