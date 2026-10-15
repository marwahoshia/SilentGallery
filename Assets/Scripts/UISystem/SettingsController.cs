using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SilentGallery.UISystem
{
    /// <summary>
    /// Drives the Settings panel: master/music volume (via the audio mixer) and
    /// mouse sensitivity, persisted across sessions with PlayerPrefs. Attach to a
    /// GameObject in the Settings panel and assign all fields in the Inspector.
    /// </summary>
    public class SettingsController : MonoBehaviour
    {
        private const string MASTER_VOLUME_KEY = "MasterVol";
        private const string MUSIC_VOLUME_KEY = "MusicVol";
        private const string MOUSE_SENSITIVITY_KEY = "MouseSens";

        private const float DEFAULT_VOLUME = 0.75f;
        private const float DEFAULT_MOUSE_SENSITIVITY = 2f;

        /// <summary>The mixer whose exposed MasterVolume/MusicVolume parameters are controlled.</summary>
        [SerializeField]
        private AudioMixer audioMixer;

        /// <summary>Slider controlling overall volume. Wire its OnValueChanged to <see cref="OnMasterVolumeChanged"/>.</summary>
        [SerializeField]
        private Slider masterVolumeSlider;

        /// <summary>Slider controlling music volume. Wire its OnValueChanged to <see cref="OnMusicVolumeChanged"/>.</summary>
        [SerializeField]
        private Slider musicVolumeSlider;

        /// <summary>Slider controlling mouse look sensitivity. Wire its OnValueChanged to <see cref="OnMouseSensitivityChanged"/>.</summary>
        [SerializeField]
        private Slider mouseSensitivitySlider;

        private void Start()
        {
            float savedMasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_VOLUME);
            float savedMusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_VOLUME);
            float savedMouseSensitivity = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_KEY, DEFAULT_MOUSE_SENSITIVITY);

            if (masterVolumeSlider != null)
            {
                masterVolumeSlider.value = savedMasterVolume;
            }
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.value = savedMusicVolume;
            }
            if (mouseSensitivitySlider != null)
            {
                mouseSensitivitySlider.value = savedMouseSensitivity;
            }

            OnMasterVolumeChanged(savedMasterVolume);
            OnMusicVolumeChanged(savedMusicVolume);
        }

        /// <summary>Applies and saves master volume. Wire to the master volume slider's OnValueChanged event.</summary>
        public void OnMasterVolumeChanged(float value)
        {
            if (audioMixer != null)
            {
                audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
            }
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, value);
        }

        /// <summary>Applies and saves music volume. Wire to the music volume slider's OnValueChanged event.</summary>
        public void OnMusicVolumeChanged(float value)
        {
            if (audioMixer != null)
            {
                audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
            }
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
        }

        /// <summary>Saves mouse sensitivity. Wire to the mouse sensitivity slider's OnValueChanged event.</summary>
        public void OnMouseSensitivityChanged(float value)
        {
            PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_KEY, value);
        }
    }
}
