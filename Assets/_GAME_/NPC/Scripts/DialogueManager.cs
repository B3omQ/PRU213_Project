using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image portraitImage;

    private bool isTyping;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string npcName, Sprite npcPortrait, string text)
    {
        nameText.text = npcName;
        portraitImage.sprite = npcPortrait;
        dialogueText.text = text;
        dialoguePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        Time.timeScale = 1f;
    }

    public bool IsActive() => dialoguePanel.activeSelf;

    public string GetCurrentText() => dialogueText.text;
}

