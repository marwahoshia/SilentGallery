using System.Collections.Generic;
using SilentGallery.ClueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SilentGallery.Cheats
{
    /// <summary>
    /// Developer cheat panel (F10). Compiled only in the Editor and development
    /// builds; in a release build the GameObject destroys itself.
    /// The UI is built in code, so no scene or prefab changes are needed.
    /// </summary>
    public class CheatManager : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        [Header("Legacy UI (old panel, hidden at runtime)")]
        [SerializeField] private GameObject panel;

        [Header("Settings")]
        [SerializeField] private ClueData[] allClues;
        [SerializeField] private KeyCode toggleKey = KeyCode.F10;

        private class Row
        {
            public ClueData clue;
            public Image background;
            public TMP_Text label;
        }

        private static readonly Color FoundColor = new Color(0.15f, 0.5f, 0.2f, 1f);
        private static readonly Color MissingColor = new Color(0.55f, 0.18f, 0.18f, 1f);
        private static readonly Color ButtonColor = new Color(0.2f, 0.25f, 0.4f, 1f);

        private readonly List<Row> rows = new List<Row>();
        private GameObject uiRoot;
        private bool isOpen;

        private void Start()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }

            BuildUi();
            uiRoot.SetActive(false);

            ClueManager manager = ClueManager.Instance;
            if (manager != null)
            {
                manager.OnClueRevealed += HandleClueChanged;
                manager.OnClueRevealedSilently += HandleClueChanged;
                manager.OnClueRemoved += HandleClueChanged;
                manager.OnCluesReset += HandleCluesReset;
                manager.OnAllRequiredCluesRevealed += HandleAllRequired;
            }
        }

        private void OnDestroy()
        {
            ClueManager manager = ClueManager.Instance;
            if (manager != null)
            {
                manager.OnClueRevealed -= HandleClueChanged;
                manager.OnClueRevealedSilently -= HandleClueChanged;
                manager.OnClueRemoved -= HandleClueChanged;
                manager.OnCluesReset -= HandleCluesReset;
                manager.OnAllRequiredCluesRevealed -= HandleAllRequired;
            }
            if (isOpen)
            {
                UIState.ClosePanel();
            }
            if (uiRoot != null)
            {
                Destroy(uiRoot);
            }
        }

        private void Update()
        {
            if (!Input.GetKeyDown(toggleKey)) return;

            if (isOpen)
            {
                Close();
            }
            else if (!UIState.IsBlocking && Time.timeScale > 0f)
            {
                Open();
            }
        }

        private void Open()
        {
            isOpen = true;
            RefreshRows();
            uiRoot.SetActive(true);
            UIState.OpenPanel();
        }

        private void Close()
        {
            if (!isOpen) return;
            isOpen = false;
            uiRoot.SetActive(false);
            UIState.ClosePanel();
        }

        private void HandleClueChanged(ClueData clue)
        {
            RefreshRows();
        }

        private void HandleCluesReset()
        {
            RefreshRows();
        }

        private void HandleAllRequired()
        {
            // Let the conclusion screen appear without the cheat panel covering it.
            Close();
        }

        private void ToggleClue(ClueData clue)
        {
            ClueManager manager = ClueManager.Instance;
            if (manager == null || clue == null) return;

            if (manager.IsClueRevealed(clue.ClueId))
            {
                manager.RemoveClue(clue);
            }
            else
            {
                manager.RevealClueSilently(clue);
            }
        }

        private void FindAll()
        {
            ClueManager manager = ClueManager.Instance;
            if (manager == null || allClues == null) return;

            foreach (ClueData clue in allClues)
            {
                manager.RevealClueSilently(clue);
            }
        }

        private void ResetProgress()
        {
            if (ClueManager.Instance != null)
            {
                ClueManager.Instance.ResetAllClues();
            }
        }

        private void RefreshRows()
        {
            ClueManager manager = ClueManager.Instance;
            foreach (Row row in rows)
            {
                bool isFound = manager != null && manager.IsClueRevealed(row.clue.ClueId);
                row.label.text = row.clue.Title + "   [" + (isFound ? "FOUND" : "NOT FOUND") + "]";
                row.background.color = isFound ? FoundColor : MissingColor;
            }
        }

        // ---------- UI built in code ----------

        private void BuildUi()
        {
            uiRoot = new GameObject("CheatCanvas");

            Canvas canvas = uiRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            CanvasScaler scaler = uiRoot.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            uiRoot.AddComponent<GraphicRaycaster>();

            RectTransform panelRect = NewRect("Panel", uiRoot.transform);
            panelRect.anchorMin = panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(760f, 800f);
            Image panelImage = panelRect.gameObject.AddComponent<Image>();
            panelImage.color = new Color(0.05f, 0.05f, 0.08f, 0.94f);

            // Title
            RectTransform titleRect = NewRect("Title", panelRect);
            titleRect.anchorMin = titleRect.anchorMax = new Vector2(0.5f, 1f);
            titleRect.pivot = new Vector2(0.5f, 1f);
            titleRect.anchoredPosition = new Vector2(0f, -20f);
            titleRect.sizeDelta = new Vector2(700f, 60f);
            TextMeshProUGUI title = titleRect.gameObject.AddComponent<TextMeshProUGUI>();
            title.text = "CHEAT MANAGER (dev only)";
            title.fontSize = 38;
            title.alignment = TextAlignmentOptions.Center;
            title.color = Color.white;
            title.raycastTarget = false;

            // Clue rows
            RectTransform listRect = NewRect("ClueList", panelRect);
            listRect.anchorMin = listRect.anchorMax = new Vector2(0.5f, 1f);
            listRect.pivot = new Vector2(0.5f, 1f);
            listRect.anchoredPosition = new Vector2(0f, -100f);
            listRect.sizeDelta = new Vector2(700f, 600f);
            VerticalLayoutGroup list = listRect.gameObject.AddComponent<VerticalLayoutGroup>();
            list.spacing = 8f;
            list.childAlignment = TextAnchor.UpperCenter;
            list.childControlWidth = true;
            list.childControlHeight = true;
            list.childForceExpandWidth = true;
            list.childForceExpandHeight = false;

            if (allClues != null)
            {
                foreach (ClueData clue in allClues)
                {
                    if (clue == null) continue;

                    ClueData captured = clue;
                    TMP_Text label;
                    Button button = NewButton("Row_" + clue.ClueId, listRect, "", FoundColor, 30, out label);
                    LayoutElement element = button.gameObject.AddComponent<LayoutElement>();
                    element.preferredHeight = 52f;
                    button.onClick.AddListener(() => ToggleClue(captured));

                    rows.Add(new Row
                    {
                        clue = clue,
                        background = button.GetComponent<Image>(),
                        label = label
                    });
                }
            }

            // Bottom buttons
            RectTransform bottomRect = NewRect("Buttons", panelRect);
            bottomRect.anchorMin = bottomRect.anchorMax = new Vector2(0.5f, 0f);
            bottomRect.pivot = new Vector2(0.5f, 0f);
            bottomRect.anchoredPosition = new Vector2(0f, 30f);
            bottomRect.sizeDelta = new Vector2(700f, 70f);
            HorizontalLayoutGroup bottom = bottomRect.gameObject.AddComponent<HorizontalLayoutGroup>();
            bottom.spacing = 15f;
            bottom.childControlWidth = true;
            bottom.childControlHeight = true;
            bottom.childForceExpandWidth = true;
            bottom.childForceExpandHeight = true;

            TMP_Text unused;
            NewButton("FindAll", bottomRect, "Find all", ButtonColor, 30, out unused)
                .onClick.AddListener(FindAll);
            NewButton("Reset", bottomRect, "Reset progress", ButtonColor, 30, out unused)
                .onClick.AddListener(ResetProgress);
            NewButton("Close", bottomRect, "Close", ButtonColor, 30, out unused)
                .onClick.AddListener(Close);
        }

        private static RectTransform NewRect(string name, Transform parent)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go.GetComponent<RectTransform>();
        }

        private static Button NewButton(string name, Transform parent, string text,
            Color background, int fontSize, out TMP_Text label)
        {
            RectTransform rect = NewRect(name, parent);
            Image image = rect.gameObject.AddComponent<Image>();
            image.color = background;
            Button button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;

            RectTransform labelRect = NewRect("Label", rect);
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            TextMeshProUGUI tmp = labelRect.gameObject.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = fontSize;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;
            tmp.raycastTarget = false;

            label = tmp;
            return button;
        }
#else
        private void Awake()
        {
            Destroy(gameObject);
        }
#endif
    }
}