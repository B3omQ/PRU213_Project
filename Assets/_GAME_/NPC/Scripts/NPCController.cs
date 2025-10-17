using System.Collections;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] private NPCDialog _dialogueData;

    private int _dialogueIndex;
    private bool _isTyping;

    public bool CanInteract()
    {
        // Only allow starting a new dialogue if no dialogue is currently active
        return !DialogueManager.Instance.IsActive();
    }

    public void Interact()
    {
        if (_dialogueData == null) return;

        if (DialogueManager.Instance.IsActive())
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        _dialogueIndex = 0;
        ShowCurrentLine();
    }

    private void NextLine()
    {
        if (_isTyping)
        {
            StopAllCoroutines();
            DialogueManager.Instance.ShowDialogue(
                _dialogueData._npcName,
                _dialogueData._npcPortrait,
                _dialogueData._dialogueLines[_dialogueIndex]
            );
            _isTyping = false;
            return;
        }

        _dialogueIndex++;
        if (_dialogueIndex < _dialogueData._dialogueLines.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine(_dialogueData._dialogueLines[_dialogueIndex]));
    }

    private IEnumerator TypeLine(string line)
    {
        _isTyping = true;
        DialogueManager.Instance.ShowDialogue(_dialogueData._npcName, _dialogueData._npcPortrait, "");

        foreach (char letter in line)
        {
            DialogueManager.Instance.ShowDialogue(
                _dialogueData._npcName,
                _dialogueData._npcPortrait,
                DialogueManager.Instance.GetCurrentText() + letter
            );
            yield return new WaitForSecondsRealtime(_dialogueData._typingSpeed);
        }

        _isTyping = false;

        if (_dialogueData._autoProgressLines.Length > _dialogueIndex && _dialogueData._autoProgressLines[_dialogueIndex])
        {
            yield return new WaitForSecondsRealtime(_dialogueData._autoProgressDelay);
            NextLine();
        }
    }

    private void EndDialogue()
    {
        DialogueManager.Instance.HideDialogue();
        StopAllCoroutines();
        _isTyping = false;
    }
}
