using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable _interactableInRange = null;
    public GameObject _interactionIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _interactionIcon.SetActive(false);
        
    }
    void Update()
    {
        if (InputManager.InteractPressed)
        {
            if (_interactableInRange != null)
            {
                _interactableInRange.Interact();

                if (!_interactableInRange.CanInteract())
                {
                    _interactionIcon.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            _interactableInRange = interactable;
            _interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == _interactableInRange)
        {
            _interactableInRange = null;
            _interactionIcon.SetActive(false);
        }
    }
}
