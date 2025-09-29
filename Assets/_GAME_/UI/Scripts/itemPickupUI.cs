using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class itemPickupUI : MonoBehaviour
{
    public static itemPickupUI Instance {  get; private set; }

    public GameObject _popUpPrefab;
    public int _maxPopUp = 5;
    public float _popUpDuration = 3f;

    private readonly Queue<GameObject> _activePopUps = new();

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else
        {
            Debug.LogError("Mutiple ItemPickupUI detected! destroying the extra one");
            Destroy(gameObject);
        }
    }

    public void ShowItemPickup(string itemName, Sprite itemIcon)
    {
        GameObject newPopup = Instantiate(_popUpPrefab, transform);
        newPopup.GetComponentInChildren<TMP_Text>().text = itemName;

        Image itemImage = newPopup.transform.Find("ItemIcon")?.GetComponent<Image>();
        if (itemImage)
        {
            itemImage.sprite = itemIcon;
        }
        _activePopUps.Enqueue(newPopup);

        if (_activePopUps.Count > _maxPopUp)
        {
            Destroy(_activePopUps.Dequeue());
        }
        StartCoroutine(FadeOutAndDestroy(newPopup));
    }
    private IEnumerator FadeOutAndDestroy(GameObject popUp) 
    {
        yield return new WaitForSeconds(_popUpDuration);
        if(popUp == null) yield break;
        
        CanvasGroup canvasGroup = popUp.GetComponent<CanvasGroup>();
        for (float timePassed = 0f; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (popUp == null) yield break;
            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }
        Destroy(popUp);
    }
    
}
