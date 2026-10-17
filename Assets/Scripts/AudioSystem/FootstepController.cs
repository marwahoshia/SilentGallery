using UnityEngine;

namespace SilentGallery.AudioSystem
{
    /// <summary>
    /// Plays a random footstep sound at regular intervals while the player is
    /// walking on the ground. Attach to the Player GameObject (which has a
    /// CharacterController) and assign the footstep clips in the Inspector.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class FootstepController : MonoBehaviour
    {
        [SerializeField] private AudioClip[] footstepClips;
        [SerializeField] private float stepInterval = 0.5f;
        [SerializeField] private float minWalkSpeed = 0.5f;
        [SerializeField, Range(0f, 1f)] private float stepVolume = 0.5f;

        private CharacterController characterController;
        private float stepTimer;
        private int lastClipIndex = -1;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (Time.timeScale == 0f)
            {
                return;
            }

            bool isWalking = characterController.isGrounded
                && characterController.velocity.magnitude > minWalkSpeed;

            if (!isWalking)
            {
                stepTimer = 0f;
                return;
            }

            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayStep();
                stepTimer = stepInterval;
            }
        }

        private void PlayStep()
        {
            if (footstepClips == null || footstepClips.Length == 0 || AudioManager.Instance == null)
            {
                return;
            }

            int index = Random.Range(0, footstepClips.Length);
            if (footstepClips.Length > 1 && index == lastClipIndex)
            {
                index = (index + 1) % footstepClips.Length;
            }
            lastClipIndex = index;

            AudioManager.Instance.PlaySFX(footstepClips[index], stepVolume);
        }
    }
}