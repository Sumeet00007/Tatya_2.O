using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{
    [Header("Patience Settings")]
    // Total time required to complete level
    public float maxPatienceTime = 10f; 
    //variable for currentpatience
    public float currentPatience = 0f;
    public GameObject player;


    [Header("UI Settings")]
    public Image patienceBar;
   

    private bool isPlayerAlive = true;

    void Start()
    {
        ResetPatienceBar();
      
    }

    void Update()
    {
        if (isPlayerAlive)
        {
            IncreasePatience(Time.deltaTime);
        }
    }

    public void ResetPatienceBar()
    {
        currentPatience = 0f;
        isPlayerAlive = true;
        UpdatePatienceBar();
    }

    // Increases patience over time
    void IncreasePatience(float amount)
    {
        currentPatience += amount;
        currentPatience = Mathf.Clamp(currentPatience, 0, maxPatienceTime +5);
        UpdatePatienceBar();

        if (currentPatience >= maxPatienceTime)
        {
            KillPlayer();
           // Debug.Log("Player Mar gaya");
        }
    }

    // Public function to decrease patience
    public void DecreasePatience(float percentage)
    {
        if (!isPlayerAlive) return;

        float decreaseAmount = (percentage / 100f) * maxPatienceTime;
        currentPatience -= decreaseAmount;
        currentPatience = Mathf.Clamp(currentPatience, 0, maxPatienceTime+5);
        UpdatePatienceBar();
    }

    // Updates UI bar
    void UpdatePatienceBar()
    {
        if (patienceBar != null)
        {
            patienceBar.fillAmount = currentPatience / maxPatienceTime;
        }
    }

    // Public function to kill player when patience is full
    public void KillPlayer()
    {
        if (isPlayerAlive)
        {
            Debug.Log("Player has died due to enemy’s patience running out!");
            isPlayerAlive = false;
            // Add game over logic here (disable movement, show UI, restart level, etc.)
            GameManager.Instance.ShowGameOver();
            Invoke(nameof(ZeroFill), 1.5f);
            
        }
    }

    private void ZeroFill()
    {
        currentPatience = 0;
        UpdatePatienceBar();
       // SetMaxPatienceTime(20.0f);
    }

    //call this fuction after completing level i.e( in 1st level after unlocking Door)
    public void SetMaxPatienceTime(float newMaxTime)
    {
        //set new max patience
        maxPatienceTime = Mathf.Max(1f, newMaxTime); // Ensure it’s at least 1 to avoid division errors
        currentPatience = Mathf.Clamp(currentPatience, 0, maxPatienceTime + 5);
        ResetPatienceBar();
    }
}
