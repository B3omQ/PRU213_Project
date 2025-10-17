using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool _isOpened {  get; private set; }
    public string _chestId {  get; private set; }

    public GameObject _itemPrefab;
    public Sprite _openedChest;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if(!CanInteract()) return;
         OpenChest();
    }

    public bool CanInteract()
    {
        return !_isOpened;
    }

    private void OpenChest()
    {
        SetOpened(true);

        if (_itemPrefab)
        {
            GameObject droppedItem = Instantiate(_itemPrefab, transform.position + Vector3.down, Quaternion.identity);
            droppedItem.GetComponent<BounceEffect>().StartBounce();
        }
    }

    public void SetOpened(bool opened)
    {
        if (_isOpened = opened)
        {
            GetComponent<SpriteRenderer>().sprite = _openedChest;
        }
    }
}
