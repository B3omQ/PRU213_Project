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
    private DialogueManager _dialogueManager;

    private enum QuestState
    {
        NotStarted,
        InProgress,
        Completed
    }
    private QuestState _questState = QuestState.NotStarted;
    private void Start()
    {
        _dialogueManager = DialogueManager.Instance;
    }
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

        SyncQuestState();
        if (_questState == QuestState.NotStarted)
        {
            _dialogueIndex = 0;
        }
        else if(_questState == QuestState.InProgress)
        {
            _dialogueIndex = _dialogueData._questInProgressIndex;
        }
        else if (_questState == QuestState.Completed)
        {
            _dialogueIndex = _dialogueData._questCompleteIndex;
        }
        ShowCurrentLine();
    }

    private void SyncQuestState()
    {
        if(_dialogueData.quest == null) return;

        string questId = _dialogueData.quest._questId;

        if (QuestController.Instance.IsQuestActive(questId))
        {
            _questState = QuestState.InProgress;
        }
        else
        {
            _questState = QuestState.NotStarted;
        }
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

        _dialogueManager.ClearChoices();

        if (_dialogueData._endDialogueLines.Length > _dialogueIndex && _dialogueData._endDialogueLines[_dialogueIndex])
        {
            EndDialogue();
            return;
        }
        foreach (DialogueChoice dialogueChoice in _dialogueData._choices)
        {
            if (dialogueChoice._dialogueIndex == _dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
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

    private void DisplayChoices(DialogueChoice choice)
    {

        Debug.Log($"[DisplayChoices] Showing choices for dialogue index {_dialogueIndex}");
        int choiceCount = choice._choices.Length;
        int nextCount = choice._nextDialogueIndexes.Length;
        int questCount = choice._givesQuest.Length;

        for (int i = 0; i < choiceCount; i++)
        {
            string text = choice._choices[i];

            int nextIndex = (i < nextCount) ? choice._nextDialogueIndexes[i] : -1;
            bool giveQuest = (i < questCount) && choice._givesQuest[i];

            _dialogueManager.CreateChoicebutton(text, () => ChoiceOption(nextIndex, giveQuest));
        }

        Canvas.ForceUpdateCanvases();
    }

    private void ChoiceOption(int nextIndex, bool giveQuest)
    {
        if (giveQuest)
        {
            QuestController.Instance.AcceptQuest(_dialogueData.quest);
            _questState = QuestState.InProgress;
        }
        _dialogueIndex = nextIndex;
        _dialogueManager.ClearChoices();
        ShowCurrentLine();
    }

    private void EndDialogue()
    {
        DialogueManager.Instance.HideDialogue();
        StopAllCoroutines();
        _isTyping = false;
    }


}
