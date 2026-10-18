using UnityEngine;

public class FallReset : MonoBehaviour
{
    public float fallY = -5f;

    private Vector3 startPosition;
    private CharacterController controller;

    void Start()
    {
        startPosition = transform.position;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (transform.position.y < fallY)
        {
            controller.enabled = false;
            transform.position = startPosition;
            controller.enabled = true;
        }
    }
}
