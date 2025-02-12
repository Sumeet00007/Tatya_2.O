using System.Collections;
using UnityEngine;
using TMPro;               // Import TextMesh Pro
using UnityEngine.Events;

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string text;               // Dialogue text
    public AudioClip voiceClip;       // Optional voice clip
    public UnityEvent onDialogueEvent; // Trigger events (animations, effects)

    [Header("Quest Settings")]
    public bool triggersQuest;        // Does this dialogue trigger a quest?
    public string questDescription;   // Quest description if triggered
    public bool isQuestCompletion;    // Marks dialogue as quest completion

    [Header("Item Requirement (Optional)")]
    public bool requiresItem;         // Does this dialogue require an item?
    public string requiredItemName;   // Name of the required item
}

public class DialogueSystem : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private DialogueLine[] dialogueLines;
    [SerializeField] private float dialogueInterval = 3f;
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("World Space UI")]
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;  // TMP Text

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;

    [Header("Quest UI")]
    [SerializeField] private TextMeshProUGUI questUIText;   // TMP Quest UI

    private int currentDialogueIndex = 0;
    private bool isTyping = false;
    private bool awaitingItem = false;

    void Start()
    {
        dialogueCanvas.worldCamera = Camera.main;
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        while (currentDialogueIndex < dialogueLines.Length)
        {
            DialogueLine currentLine = dialogueLines[currentDialogueIndex];

            yield return StartCoroutine(TypeDialogue(currentLine.text));

            if (currentLine.voiceClip != null)
                audioSource.PlayOneShot(currentLine.voiceClip);

            currentLine.onDialogueEvent.Invoke();

            if (currentLine.triggersQuest)
                UpdateQuestUI(currentLine.questDescription);

            if (currentLine.requiresItem && !awaitingItem)
            {
                awaitingItem = true;
                yield return new WaitUntil(() => HasReceivedItem(currentLine.requiredItemName));
                awaitingItem = false;
            }

            if (currentLine.isQuestCompletion)
            {
                UpdateQuestUI("Quest Completed!");
                break;
            }

            currentDialogueIndex++;
            yield return new WaitForSeconds(dialogueInterval);
        }
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private bool HasReceivedItem(string itemName)
    {
        // Placeholder for item-checking logic
        return false;
    }

    public void ReceiveItem(string itemName)
    {
        if (awaitingItem && dialogueLines[currentDialogueIndex].requiredItemName == itemName)
        {
            awaitingItem = false;
        }
    }

    private void UpdateQuestUI(string questDescription)
    {
        if (questUIText != null)
            questUIText.text = $"<b>Quest Updated:</b> {questDescription}"; // Bold formatting with TMP
    }
}
