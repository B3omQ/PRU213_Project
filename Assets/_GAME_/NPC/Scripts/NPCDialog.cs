using UnityEngine;

[CreateAssetMenu(fileName ="NewNPCDialogue", menuName ="NPC Dialogue")]
public class NPCDialog : ScriptableObject
{
    public string _npcName;
    public Sprite _npcPortrait;
    public string[] _dialogueLines;
    public bool[] _autoProgressLines;
    public float _typingSpeed = 0.05f;
    public AudioClip _voiceSound;
    public float _voicePitch = 1f;
    public float _autoProgressDelay = 1.5f;
}
