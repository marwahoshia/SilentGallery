using System.Collections.Generic;
using UnityEngine;

namespace SilentGallery.InteractionSystem
{
    /// <summary>
    /// Highlights an interactable while the player looks at it.
    /// Uses emission when the shader supports it, otherwise brightens the main color
    /// (needed for legacy shaders such as Bumped Diffuse).
    /// Added automatically by InteractionController, so no scene edits are needed.
    /// </summary>
    public class InteractHighlight : MonoBehaviour
    {
        [SerializeField] private Color emissionColor = new Color(1f, 0.85f, 0.4f);
        [SerializeField] private Color tintMultiplier = new Color(1.8f, 1.6f, 1.2f);

        private struct SavedMaterial
        {
            public Material material;
            public bool usedEmission;
            public bool hadEmission;
            public Color color;
        }

        private readonly List<SavedMaterial> saved = new List<SavedMaterial>();
        private bool isOn;

        public void SetHighlighted(bool value)
        {
            if (value == isOn) return;
            isOn = value;
            if (isOn) Apply(); else Restore();
        }

        private void Apply()
        {
            saved.Clear();
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                if (!(r is MeshRenderer || r is SkinnedMeshRenderer)) continue;
                foreach (Material m in r.materials)
                {
                    if (m.HasProperty("_EmissionColor"))
                    {
                        saved.Add(new SavedMaterial
                        {
                            material = m,
                            usedEmission = true,
                            hadEmission = m.IsKeywordEnabled("_EMISSION"),
                            color = m.GetColor("_EmissionColor")
                        });
                        m.EnableKeyword("_EMISSION");
                        m.SetColor("_EmissionColor", emissionColor);
                    }
                    else if (m.HasProperty("_Color"))
                    {
                        Color original = m.GetColor("_Color");
                        saved.Add(new SavedMaterial
                        {
                            material = m,
                            usedEmission = false,
                            color = original
                        });
                        m.SetColor("_Color", original * tintMultiplier);
                    }
                }
            }
        }

        private void Restore()
        {
            foreach (SavedMaterial s in saved)
            {
                if (s.material == null) continue;
                if (s.usedEmission)
                {
                    s.material.SetColor("_EmissionColor", s.color);
                    if (!s.hadEmission) s.material.DisableKeyword("_EMISSION");
                }
                else
                {
                    s.material.SetColor("_Color", s.color);
                }
            }
            saved.Clear();
        }

        private void OnDisable()
        {
            if (isOn)
            {
                Restore();
                isOn = false;
            }
        }
    }
}