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

    [Header("Player Interaction")]
    [SerializeField] private Transform itemContainer;  // Reference to player's item holder
    [SerializeField] private float interactionRange = 2f; // Range for detecting player

    private int currentDialogueIndex = 0;
    //private bool isTyping = false;
    private bool awaitingItem = false;
    private Player player; // Reference to Player

    void Start()
    {
        dialogueCanvas.worldCamera = Camera.main;
        player = FindFirstObjectByType<Player>(); // Find player by tag
        StartCoroutine(PlayDialogue());
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (awaitingItem && distance <= interactionRange && Input.GetKeyDown(KeyCode.E))
        {
            TryToReceiveItem();
        }
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
                //dialogueText.text = "Bring me a " + currentLine.requiredItemName + "!"; // Notify player
                yield return new WaitUntil(() => !awaitingItem); // Wait for item delivery
            }

            if (currentLine.isQuestCompletion)
            {
                UpdateQuestUI("Quest Completed!");
            }

            // Move to the next dialogue, even if a quest was completed
            currentDialogueIndex++;
            yield return new WaitForSeconds(dialogueInterval);
        }
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        //isTyping = true;
        dialogueText.text = "";

        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        //isTyping = false;
    }

    private void TryToReceiveItem()
    {
        if (itemContainer.childCount > 0) // Player has an item
        {
            Transform item = itemContainer.GetChild(0);
            if (item.name == dialogueLines[currentDialogueIndex].requiredItemName)
            {
                Destroy(item.gameObject); // Remove item from player
                player.isHandsFree = true;
                awaitingItem = false; // Continue dialogue
                //dialogueText.text = "Thank you!";
            }
            else
            {
                dialogueText.text = "This is not what I asked for!";
            }
        }
        else
        {
            dialogueText.text = "You don't have anything to give!";
        }
    }

    private void UpdateQuestUI(string questDescription)
    {
        if (questUIText != null)
            questUIText.text = $"<b>Quest Updated:</b> {questDescription}"; // Bold formatting with TMP
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange); // Show player detection range
    }
}