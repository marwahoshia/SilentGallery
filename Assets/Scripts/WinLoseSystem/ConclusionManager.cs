using System.Collections;
using SilentGallery.ClueSystem;
using SilentGallery.WinLoseSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConclusionManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private int correctAnswerIndex;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private GameOutcomeManager gameOutcomeManager;
    [SerializeField] private ClueDisplayUI clueDisplayUI;

    [SerializeField] private int maxWrongAttempts = 3;
    [SerializeField] private float penaltySeconds = 30f;

    private int wrongAttempts;
    private bool isPenalized;
    private Coroutine penaltyCoroutine;

    private void Awake()
    {
        if (answerButtons == null)
        {
            return;
        }

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            if (answerButtons[i] != null)
            {
                answerButtons[i].onClick.AddListener(() => HandleAnswerSelected(index));
            }
        }
    }

    private void Start()
    {
        if (gameOutcomeManager != null)
        {
            gameOutcomeManager.OnGameLost += HandleGameEndedExternally;
        }
    }

    private void OnDestroy()
    {
        if (gameOutcomeManager != null)
        {
            gameOutcomeManager.OnGameLost -= HandleGameEndedExternally;
        }
    }

    // The countdown timer can still end the game while this panel is open
    // (HandleAllCluesRevealed no longer stops it) - close ourselves so the
    // end screen isn't stuck showing underneath/behind this one.
    private void HandleGameEndedExternally()
    {
        ClosePanel();
    }

    public void ShowConclusion()
    {
        clueDisplayUI?.CloseClueDisplay();

        wrongAttempts = 0;
        isPenalized = false;
        SetButtonsInteractable(true);
        if (feedbackText != null)
        {
            feedbackText.text = string.Empty;
        }
        if (panel != null)
        {
            panel.SetActive(true);
        }
        UIState.OpenPanel();
    }

    private void HandleAnswerSelected(int index)
    {
        if (isPenalized)
        {
            return;
        }

        if (index == correctAnswerIndex)
        {
            if (feedbackText != null)
            {
                feedbackText.text = "Correct!";
            }
            ClosePanel();
            if (gameOutcomeManager != null)
            {
                gameOutcomeManager.TriggerWin();
            }
            return;
        }

        wrongAttempts++;
        if (wrongAttempts >= maxWrongAttempts)
        {
            if (feedbackText != null)
            {
                feedbackText.text = "Wrong. Out of attempts.";
            }
            ClosePanel();
            if (gameOutcomeManager != null)
            {
                gameOutcomeManager.TriggerLose();
            }
            return;
        }

        penaltyCoroutine = StartCoroutine(PenaltyCooldown());
    }

    private IEnumerator PenaltyCooldown()
    {
        isPenalized = true;
        SetButtonsInteractable(false);

        int attemptsLeft = maxWrongAttempts - wrongAttempts;
        float remaining = penaltySeconds;
        while (remaining > 0f)
        {
            if (feedbackText != null)
            {
                feedbackText.text = $"Wrong. {attemptsLeft} attempt(s) left. Try again in {Mathf.CeilToInt(remaining)}s";
            }
            yield return null;
            remaining -= Time.deltaTime;
        }

        if (feedbackText != null)
        {
            feedbackText.text = string.Empty;
        }
        isPenalized = false;
        SetButtonsInteractable(true);
        penaltyCoroutine = null;
    }

    private void SetButtonsInteractable(bool interactable)
    {
        if (answerButtons == null)
        {
            return;
        }

        foreach (Button button in answerButtons)
        {
            if (button != null)
            {
                button.interactable = interactable;
            }
        }
    }

    private void ClosePanel()
    {
        if (penaltyCoroutine != null)
        {
            StopCoroutine(penaltyCoroutine);
            penaltyCoroutine = null;
        }

        if (panel != null && panel.activeSelf)
        {
            panel.SetActive(false);
            UIState.ClosePanel();
        }
    }
}
