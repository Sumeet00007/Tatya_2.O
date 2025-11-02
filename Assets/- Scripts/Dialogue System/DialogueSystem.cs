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
        public AudioClip itemGive;
        public AudioClip itemReject;
        public float dialogueVolume = 1f;
        public float volumeMultiplier = 2f;

        [Header("Ambient Horror System")]
        [SerializeField] private AudioSource ambientSource;
        [SerializeField] private AudioClip[] ambientClips;
        [SerializeField] private float ambientStartDelay = 10f;
        [SerializeField] private float ambientInterval = 30f;
        private bool ambientActive = false;
        private Coroutine ambientCoroutine;

        [Header("Quest UI")]
        [SerializeField] private TextMeshProUGUI questUIText;
        [SerializeField] private float questFadeDuration = 1f;
        [SerializeField] private float questDisplayTime = 3f;
        [SerializeField] private AudioClip questAppearSFX;
        private Coroutine questFadeCoroutine;

        [Header("Player Interaction")]
        [SerializeField] private Transform itemContainer;
        [SerializeField] private float interactionRange = 2f;

        private int currentDialogueIndex = 0;
        private bool awaitingItem = false;
        private bool isPaused = false;
        private bool isDialogueActive = true;
        private Player player;

        private Coroutine dialogueCoroutine;

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
            if (awaitingItem && distance <= interactionRange && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)))
            {
                TryToReceiveItem();
            }
        }

        // ✅ Publicly callable function to activate ambient system
        public void ActivateAmbientHorror()
        {
            if (!ambientActive)
            {
                ambientActive = true;
                Debug.Log("Ambient Horror System Activated.");
            }
        }

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
                yield return WaitIfPaused();

                DialogueLine currentLine = dialogueLines[currentDialogueIndex];

                if (ambientSource && ambientSource.isPlaying)
                    ambientSource.Stop(); // Stop ambient when dialogue starts

                if (currentLine.voiceClip != null)
                    audioSource.PlayOneShot(currentLine.voiceClip, dialogueVolume * volumeMultiplier);

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

                if (currentLine.voiceClip != null)
                    yield return new WaitWhile(() => audioSource.isPlaying);

                // ✅ After each dialogue finishes
                if (ambientActive)
                {
                    if (ambientCoroutine != null)
                        StopCoroutine(ambientCoroutine);
                    ambientCoroutine = StartCoroutine(RestartAmbientAfterDelay());
                }

                currentDialogueIndex++;
                yield return new WaitForSeconds(dialogueInterval);
            }
        }

        IEnumerator TypeDialogue(string dialogue)
        {
            dialogueText.text = "";
            foreach (char letter in dialogue.ToCharArray())
            {
                yield return WaitIfPaused();
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
                    audioSource.PlayOneShot(itemGive);
                }
                else
                {
                    StartCoroutine(ShowTemporaryLine("This is not what I asked for!"));
                    audioSource.PlayOneShot(itemReject);
                }
            }
            else
            {
                StartCoroutine(ShowTemporaryLine("You don't have anything to give!"));
                audioSource.PlayOneShot(itemReject);
            }
        }

        private IEnumerator ShowTemporaryLine(string tempText)
        {
            string previousText = dialogueText.text;
            dialogueText.text = tempText;
            yield return new WaitForSeconds(2f);
            dialogueText.text = previousText;
        }

        private void UpdateQuestUI(string questDescription)
        {
            if (questUIText == null) return;

            if (questFadeCoroutine != null)
                StopCoroutine(questFadeCoroutine);

            questFadeCoroutine = StartCoroutine(FadeQuestTextRoutine($"<b>Quest Updated:</b> {questDescription}"));
        }

        private IEnumerator FadeQuestTextRoutine(string text)
        {
            questUIText.text = text;
            questUIText.gameObject.SetActive(true);
            Color c = questUIText.color;
            c.a = 0;
            questUIText.color = c;

            audioSource.PlayOneShot(questAppearSFX);

            float t = 0f;
            while (t < questFadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(0f, 1f, t / questFadeDuration);
                questUIText.color = c;
                yield return null;
            }

            yield return new WaitForSeconds(questDisplayTime);

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

        // ✅ Ambient System Routine
        private IEnumerator AmbientRoutine()
        {
            int index = 0;
            while (ambientActive)
            {
                if (!audioSource.isPlaying && ambientClips.Length > 0)
                {
                    ambientSource.clip = ambientClips[index];
                    ambientSource.Play();

                    index = (index + 1) % ambientClips.Length;
                }

                yield return new WaitForSeconds(ambientInterval);
            }
        }

        // ✅ Wait before restarting ambient after each dialogue
        private IEnumerator RestartAmbientAfterDelay()
        {
            yield return new WaitForSeconds(ambientStartDelay);

            if (ambientActive)
            {
                ambientCoroutine = StartCoroutine(AmbientRoutine());
            }
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
            yield return new WaitWhile(() => !isDialogueActive);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
