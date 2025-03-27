//using System.Collections;
//using UnityEngine;
//using TMPro;
//using UnityEngine.Events;

//namespace MyGame.Dialogue
//{
//    [System.Serializable]
//    public class DialogueLine
//    {
//        [TextArea(2, 5)]
//        public string text;
//        public AudioClip voiceClip;
//        public UnityEvent onDialogueEvent;

//        [Header("Quest Settings")]
//        public bool triggersQuest;
//        public string questDescription;
//        public bool isQuestCompletion;

//        [Header("Item Requirement (Optional)")]
//        public bool requiresItem;
//        public string requiredItemName;
//    }

//    public class DialogueSystem : MonoBehaviour
//    {
//        [Header("Dialogue Settings")]
//        [SerializeField] private DialogueLine[] dialogueLines;
//        [SerializeField] private float dialogueInterval = 3f;
//        [SerializeField] private float typingSpeed = 0.05f;

//        [Header("World Space UI")]
//        [SerializeField] private Canvas dialogueCanvas;
//        [SerializeField] private TextMeshProUGUI dialogueText;

//        [Header("Audio Settings")]
//        [SerializeField] private AudioSource audioSource;
//        public float dialogueVolume = 1f;
//        public float volumeMultiplier = 2f;

//        [Header("Quest UI")]
//        [SerializeField] private TextMeshProUGUI questUIText;

//        [Header("Player Interaction")]
//        [SerializeField] private Transform itemContainer;
//        [SerializeField] private float interactionRange = 2f;

//        private int currentDialogueIndex = 0;
//        private bool awaitingItem = false;
//        private Player player;

//        void Start()
//        {
//            dialogueCanvas.worldCamera = Camera.main;
//            player = FindFirstObjectByType<Player>();

//            if (player == null)
//            {
//                Debug.LogError("Player not found! Make sure there is a Player script in the scene.");
//                return;
//            }

//            StartCoroutine(PlayDialogue());
//        }

//        void Update()
//        {
//            if (player == null) return;

//            float distance = Vector3.Distance(transform.position, player.transform.position);
//            if (awaitingItem && distance <= interactionRange && Input.GetKeyDown(KeyCode.E))
//            {
//                TryToReceiveItem();
//            }
//        }

//        IEnumerator PlayDialogue()
//        {
//            while (currentDialogueIndex < dialogueLines.Length)
//            {
//                DialogueLine currentLine = dialogueLines[currentDialogueIndex];

//                if (currentLine.voiceClip != null)
//                    audioSource.PlayOneShot(currentLine.voiceClip, dialogueVolume * volumeMultiplier);

//                yield return StartCoroutine(TypeDialogue(currentLine.text));



//                currentLine.onDialogueEvent.Invoke();

//                if (currentLine.triggersQuest)
//                    UpdateQuestUI(currentLine.questDescription);

//                if (currentLine.requiresItem && !awaitingItem)
//                {
//                    awaitingItem = true;
//                    yield return new WaitUntil(() => !awaitingItem);
//                }

//                if (currentLine.isQuestCompletion)
//                {
//                    UpdateQuestUI("Quest Completed!");
//                }

//                currentDialogueIndex++;
//                yield return new WaitForSeconds(dialogueInterval);
//            }
//        }

//        IEnumerator TypeDialogue(string dialogue)
//        {
//            dialogueText.text = "";

//            foreach (char letter in dialogue.ToCharArray())
//            {
//                dialogueText.text += letter;
//                yield return new WaitForSeconds(typingSpeed);
//            }
//        }

//        private void TryToReceiveItem()
//        {
//            if (itemContainer.childCount > 0)
//            {
//                Transform item = itemContainer.GetChild(0);
//                if (item.name == dialogueLines[currentDialogueIndex].requiredItemName)
//                {
//                    Destroy(item.gameObject);
//                    player.isHandsFree = true;
//                    awaitingItem = false;
//                }
//                else
//                {
//                    dialogueText.text = "This is not what I asked for!";
//                }
//            }
//            else
//            {
//                dialogueText.text = "You don't have anything to give!";
//            }
//        }

//        private void UpdateQuestUI(string questDescription)
//        {
//            if (questUIText != null)
//                questUIText.text = $"<b>Quest Updated:</b> {questDescription}";
//        }

//        void OnDrawGizmos()
//        {
//            Gizmos.color = Color.green;
//            Gizmos.DrawWireSphere(transform.position, interactionRange);
//        }
//    }
//}


using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace MyGame.Dialogue
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(2, 5)]
        public string text;
        public AudioClip voiceClip;
        public UnityEvent onDialogueEvent;

        [Header("Quest Settings")]
        public bool triggersQuest;
        public string questDescription;
        public bool isQuestCompletion;

        [Header("Item Requirement (Optional)")]
        public bool requiresItem;
        public string requiredItemName;
    }

    public class DialogueSystem : MonoBehaviour
    {
        [Header("Dialogue Settings")]
        [SerializeField] private DialogueLine[] dialogueLines;
        [SerializeField] private float dialogueInterval = 3f;
        [SerializeField] private float typingSpeed = 0.05f;

        [Header("World Space UI")]
        [SerializeField] private Canvas dialogueCanvas;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [Header("Audio Settings")]
        [SerializeField] private AudioSource audioSource;
        public float dialogueVolume = 1f;
        public float volumeMultiplier = 2f;

        [Header("Quest UI")]
        [SerializeField] private TextMeshProUGUI questUIText;

        [Header("Player Interaction")]
        [SerializeField] private Transform itemContainer;
        [SerializeField] private float interactionRange = 2f;

        private int currentDialogueIndex = 0;
        private bool awaitingItem = false;
        private Player player;

        void Start()
        {
            dialogueCanvas.worldCamera = Camera.main;
            player = FindFirstObjectByType<Player>();

            if (player == null)
            {
                Debug.LogError("Player not found! Make sure there is a Player script in the scene.");
                return;
            }

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

                if (currentLine.voiceClip != null)
                {
                    audioSource.PlayOneShot(currentLine.voiceClip, dialogueVolume * volumeMultiplier);
                }

                yield return StartCoroutine(TypeDialogue(currentLine.text));

                currentLine.onDialogueEvent.Invoke();

                if (currentLine.triggersQuest)
                    UpdateQuestUI(currentLine.questDescription);

                if (currentLine.requiresItem && !awaitingItem)
                {
                    awaitingItem = true;
                    yield return new WaitUntil(() => !awaitingItem);
                }

                if (currentLine.isQuestCompletion)
                {
                    UpdateQuestUI("Quest Completed!");
                }

                // Wait for voice clip to finish before proceeding
                if (currentLine.voiceClip != null)
                {
                    yield return new WaitForSeconds(currentLine.voiceClip.length);
                }
                else
                {
                    yield return new WaitForSeconds(dialogueInterval);
                }

                currentDialogueIndex++;
            }
        }

        IEnumerator TypeDialogue(string dialogue)
        {
            dialogueText.text = "";

            foreach (char letter in dialogue.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        private void TryToReceiveItem()
        {
            if (itemContainer.childCount > 0)
            {
                Transform item = itemContainer.GetChild(0);
                if (item.name == dialogueLines[currentDialogueIndex].requiredItemName)
                {
                    Destroy(item.gameObject);
                    player.isHandsFree = true;
                    awaitingItem = false;
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
                questUIText.text = $"<b>Quest Updated:</b> {questDescription}";
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
