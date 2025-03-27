using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shadow : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private DialogueSystem.DialogueLine[] dialogueLines;
    [SerializeField] private float dialogueInterval = 3f;
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("World Space UI")]
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;

    private int currentDialogueIndex = 0;
    private bool hasPlayed = false; // To ensure it only plays once

    private void Start()
    {
        dialogueText.text = " ";
    }

    public void StartDialogue()
    {
        if (!hasPlayed)
        {
            hasPlayed = true; // Prevent replaying
            dialogueCanvas.gameObject.SetActive(true); // Show dialogue UI
            StartCoroutine(PlayDialogue());
        }
    }

    IEnumerator PlayDialogue()
    {
        while (currentDialogueIndex < dialogueLines.Length)
        {
            DialogueSystem.DialogueLine currentLine = dialogueLines[currentDialogueIndex];
            yield return StartCoroutine(TypeDialogue(currentLine.GetText()));

            if (currentLine.GetVoiceClip() != null)
                audioSource.PlayOneShot(currentLine.GetVoiceClip());

            currentDialogueIndex++;
            yield return new WaitForSeconds(dialogueInterval);
        }

        // Hide dialogue UI and clear text after completion
        dialogueText.text = "";
        dialogueCanvas.gameObject.SetActive(false);
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
}

// Encapsulate DialogueLine in a namespace to avoid conflicts
namespace DialogueSystem
{
    [System.Serializable]
    public class DialogueLine
    {
        [SerializeField] private string text;
        [SerializeField] private AudioClip voiceClip;

        public string GetText() => text;
        public AudioClip GetVoiceClip() => voiceClip;
    }
}
