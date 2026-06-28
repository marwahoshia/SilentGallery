using UnityEngine;

namespace SilentGallery.PlayerMovement
{
    /// <summary>
    /// Custom first-person movement: WASD/arrow-key walking, mouse look,
    /// jumping, and gravity, all driven by a <see cref="CharacterController"/>.
    /// Attach to the Player GameObject; attach the player's Camera as a child
    /// and assign it to <see cref="cameraTransform"/> for vertical (pitch) look.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        /// <summary>Walking speed in meters per second.</summary>
        [SerializeField]
        private float moveSpeed = 4f;

        /// <summary>Initial upward velocity applied when jumping, in meters per second.</summary>
        [SerializeField]
        private float jumpSpeed = 5f;

        /// <summary>Downward acceleration applied every frame, in meters per second squared.</summary>
        [SerializeField]
        private float gravity = -9.81f;

        /// <summary>Mouse look sensitivity. Higher values turn the camera faster.</summary>
        [SerializeField]
        private float mouseSensitivity = 2f;

        /// <summary>
        /// The child Transform holding the player's Camera. Vertical (pitch) look
        /// rotates this transform; horizontal (yaw) look rotates the player body.
        /// Assign in the Inspector.
        /// </summary>
        [SerializeField]
        private Transform cameraTransform;

        /// <summary>Clamps how far up/down the camera can pitch, in degrees.</summary>
        [SerializeField]
        private float maxLookAngle = 80f;

        private CharacterController characterController;
        private float verticalVelocity;
        private float cameraPitch;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMouseLook();
            HandleMovement();
        }

        private void HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);

            cameraPitch -= mouseY;
            cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);
            if (cameraTransform != null)
            {
                cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
            }
        }

        private void HandleMovement()
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");
            Vector3 horizontalMove = (transform.right * inputX + transform.forward * inputZ) * moveSpeed;

            if (characterController.isGrounded)
            {
                verticalVelocity = 0f;
                if (Input.GetButtonDown("Jump"))
                {
                    verticalVelocity = jumpSpeed;
                }
            }

            verticalVelocity += gravity * Time.deltaTime;

            Vector3 totalMove = horizontalMove + Vector3.up * verticalVelocity;
            characterController.Move(totalMove * Time.deltaTime);
        }
    }
}
