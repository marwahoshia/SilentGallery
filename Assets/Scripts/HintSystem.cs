using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SilentGallery.TimerSystem;

public class HintSystem : MonoBehaviour
{
    [Header("References")]
    public CombinationLock combinationLock;
    public CountdownTimer countdownTimer;

    [Header("UI")]
    public Button hintButton;
    public TextMeshProUGUI hintButtonText;
    public TextMeshProUGUI hintText;

    [Header("Settings")]
    public int hintsAvailable = 2;
    public float hintCost = 30f;

    private int hintsUsed = 0;
    private string revealedHints = "";
    private string[] positions = { "first", "second", "third" };

    void Start()
    {
        hintText.text = "";
        UpdateButton();
    }

    public void RequestHint()
    {
        if (hintsUsed >= hintsAvailable)
        {
            return;
        }

        if (countdownTimer.RemainingSeconds <= hintCost)
        {
            hintText.text = "Not enough time for a hint!";
            return;
        }

        countdownTimer.AddTime(-hintCost);

        int digit = combinationLock.secretCode[hintsUsed];
        revealedHints += "The " + positions[hintsUsed] + " number is " + digit + "\n";
        hintText.text = revealedHints;

        hintsUsed++;
        UpdateButton();
    }

    void UpdateButton()
    {
        int hintsLeft = hintsAvailable - hintsUsed;

        if (hintsLeft > 0)
        {
            hintButtonText.text = "Hint (-" + hintCost + " sec) : " + hintsLeft + " left";
        }
        else
        {
            hintButtonText.text = "No hints left";
            hintButton.interactable = false;
        }
    }
}