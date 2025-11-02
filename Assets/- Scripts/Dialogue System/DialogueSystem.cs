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
        public AudioSource itemGive;
        public AudioSource itemReject;
        public float dialogueVolume = 1f;
        public float volumeMultiplier = 2f;

        [Header("Quest UI")]
        [SerializeField] private TextMeshProUGUI questUIText;
        [SerializeField] private float questFadeDuration = 1f;    // fade in/out speed
        [SerializeField] private float questDisplayTime = 3f;     // how long text stays visible
        [SerializeField] private AudioClip questAppearSFX;
        private Coroutine questFadeCoroutine;                     // track current fade

        [Header("Player Interaction")]
        [SerializeField] private Transform itemContainer;
        [SerializeField] private float interactionRange = 2f;

        private int currentDialogueIndex = 0;
        private bool awaitingItem = false;
        private bool isPaused = false;          // ✅ new
        private bool isDialogueActive = true;
        private Player player;

        private Coroutine dialogueCoroutine;    // ✅ track main coroutine

        void Start()
        {
            dialogueCanvas.worldCamera = Camera.main;
            player = FindFirstObjectByType<Player>();

            if (player == null)
            {
                Debug.LogError("Player not found!");
                return;
            }

            dialogueCoroutine = StartCoroutine(PlayDialogue());
        }

        void Update()
        {
            if (player == null || isPaused) return;

            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (awaitingItem && distance <= interactionRange && Input.GetKeyDown(KeyCode.E))
            {
                TryToReceiveItem();
            }
        }

        // ✅ PUBLIC API for Pause/Resume called by PauseMenu
        public void PauseDialogue()
        {
            if (isPaused) return;
            isPaused = true;
        }

        public void ResumeDialogue()
        {
            if (!isPaused) return;
            isPaused = false;
        }

        IEnumerator PlayDialogue()
        {
            while (currentDialogueIndex < dialogueLines.Length)
            {
                yield return WaitIfPaused(); // ⬅ pauses when game is paused

                DialogueLine currentLine = dialogueLines[currentDialogueIndex];

                if (currentLine.voiceClip != null)
                    audioSource.PlayOneShot(currentLine.voiceClip, dialogueVolume * volumeMultiplier);

                yield return StartCoroutine(TypeDialogue(currentLine.text));

                if (currentLine.triggersQuest)
                    UpdateQuestUI(currentLine.questDescription);

                currentLine.onDialogueEvent.Invoke();

                if (currentLine.requiresItem && !awaitingItem)
                {
                    awaitingItem = true;
                    yield return new WaitUntil(() => !awaitingItem);
                }

                if (currentLine.isQuestCompletion)
                {
                    UpdateQuestUI("Quest Completed!");
                }

                if (currentLine.voiceClip != null)
                    yield return new WaitWhile(() => audioSource.isPlaying);

                currentDialogueIndex++;
               yield return new WaitForSeconds(dialogueInterval);
               
            }
        }

        IEnumerator TypeDialogue(string dialogue)
        {
            dialogueText.text = "";
            foreach (char letter in dialogue.ToCharArray())
            {
                yield return WaitIfPaused(); // ⬅ pauses typing when game is paused
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
                    itemGive.Play();
                }
                else
                {
                    dialogueText.text = "This is not what I asked for!";
                    itemReject.Play();
                }
            }
            else
            {
                dialogueText.text = "You don't have anything to give!";
                itemReject.Play();
            }
        }

        private void UpdateQuestUI(string questDescription)
        {
            if (questUIText == null) return;

            // If another fade is running, stop it first
            if (questFadeCoroutine != null)
                StopCoroutine(questFadeCoroutine);

            questFadeCoroutine = StartCoroutine(FadeQuestTextRoutine($"<b>Quest Updated:</b> {questDescription}"));
        }

        private IEnumerator FadeQuestTextRoutine(string text)
        {
            questUIText.text = text;
            questUIText.gameObject.SetActive(true);

            // Start from transparent
            Color c = questUIText.color;
            c.a = 0;
            questUIText.color = c;

            // Fade In
            float t = 0f;

            audioSource.PlayOneShot(questAppearSFX);

            while (t < questFadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(0f, 1f, t / questFadeDuration);
                questUIText.color = c;
                yield return null;
            }

            // Wait visible
            yield return new WaitForSeconds(questDisplayTime);

            // Fade Out
            t = 0f;

            audioSource.PlayOneShot(questAppearSFX);

            while (t < questFadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(1f, 0f, t / questFadeDuration);
                questUIText.color = c;
                yield return null;
            }

            questUIText.gameObject.SetActive(false);
        }


        public void ResetToCheckpoint(int checkpointIndex)
        {
            currentDialogueIndex = checkpointIndex;
            StopAllCoroutines();
            dialogueCoroutine = StartCoroutine(PlayDialogue());
            Debug.Log("Dialogue resumed from checkpoint: " + checkpointIndex);
        }

        public void SetDialogueActive(bool active)
        {
            isDialogueActive = active;
        }

        private IEnumerator WaitIfPaused()
        {
            // Used inside dialogue coroutines to pause typing/dialogue when game is paused
            yield return new WaitWhile(() => !isDialogueActive);
        }


        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
