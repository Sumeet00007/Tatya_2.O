using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Over UI")]
    public GameObject gameOverImage;
    public GameObject gameOverCutScene;
    public float gameOverDelayforCutscene = 0.5f;
    public float gameOverDelayforImg = 1f;

    [Header("Dialogue Checkpoint System")]
    [Tooltip("How many times checkpoint can be reused before resetting dialogue to beginning.")]
    public int maxCheckpointUses = 3;
    private int dialogueCheckpointIndex = 0;  // Last dialogue index
    private int checkpointUses = 0;           // Count of checkpoint uses

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (gameOverImage != null) gameOverImage.SetActive(false);
        if (gameOverCutScene != null) gameOverCutScene.SetActive(false);
    }

    // Update checkpoint
    public void UpdateDialogueCheckpoint(int index)
    {
        dialogueCheckpointIndex = index;
        Debug.Log($"✅ Checkpoint Updated at Dialogue Index: {dialogueCheckpointIndex}");
    }

    // Return checkpoint index for DialogueSystem
    public int GetCheckpointIndex()
    {
        if (checkpointUses >= maxCheckpointUses)
        {
            Debug.Log("⚠ Checkpoint limit reached! Restarting dialogue from beginning.");
            ResetCheckpoint();
            return 0;
        }

        checkpointUses++;
        Debug.Log($"➡ Using Checkpoint #{checkpointUses} | Start From Index: {dialogueCheckpointIndex}");
        return dialogueCheckpointIndex;
    }

    public void ResetCheckpoint()
    {
        dialogueCheckpointIndex = 0;
        checkpointUses = 0;
        Debug.Log("🔁 Dialogue Checkpoint System Reset!");
    }

    // Called on player death
    public void ShowGameOver()
    {
        Invoke(nameof(DisplayGameOver), gameOverDelayforCutscene);
    }

    private void DisplayGameOver()
    {
        if (gameOverCutScene != null)
            gameOverCutScene.SetActive(true);

        Invoke(nameof(RestartLevel), 4.5f);
    }

    private void RestartLevel()
    {
        bool retriesExceeded = checkpointUses >= maxCheckpointUses;

        if (retriesExceeded)
        {
            // Reload full scene
            Debug.Log("Checkpoint limit exceeded. Reloading full scene.");
            dialogueCheckpointIndex = 0;
            checkpointUses = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            // Restart dialogue from last checkpoint
            Debug.Log($"Restarting dialogue from checkpoint {dialogueCheckpointIndex}");
            MyGame.Dialogue.DialogueSystem dialogueSystem = GameObject.FindGameObjectWithTag("Tatya").GetComponent<MyGame.Dialogue.DialogueSystem>();
            if (dialogueSystem != null)
            {
                dialogueSystem.ResetToCheckpoint(dialogueCheckpointIndex);
            }

            if (gameOverCutScene != null) gameOverCutScene.SetActive(false);
            if (gameOverImage != null) gameOverImage.SetActive(false);
        }
    }

    public void ShowGameOverImage()
    {
        if (gameOverImage == null) return;

        gameOverImage.SetActive(true);
        Invoke(nameof(RestartLevel), gameOverDelayforImg);
    }

    public void GameOverCutscene()
    {
        SceneManager.LoadScene("Ending CutScene");
    }
}
