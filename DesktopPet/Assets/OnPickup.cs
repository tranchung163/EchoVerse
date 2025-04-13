using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class OnPickup : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private UnityEvent OnBlueberry;
    [SerializeField] AudioSource audioSource;

    private InputAction touchActionPosition;
    private InputAction touchActionPressed;

    private void OnEnable()
    {
        if (playerInput == null) return;

        // Get the input actions
        touchActionPosition = playerInput.actions["Look"];
        touchActionPressed = playerInput.actions["Attack"];

        if (touchActionPressed != null)
        {
            touchActionPressed.performed += TouchPressed;
            touchActionPressed.Enable();
        }
        else
        {
            Debug.LogError("Could not find 'Attack' action.");
        }

        if (touchActionPosition != null)
        {
            touchActionPosition.Enable();
        }
        else
        {
            Debug.LogError("Could not find 'TouchPosition' action.");
        }
    }

    private void OnDisable()
    {
        if (touchActionPressed != null)
        {
            touchActionPressed.performed -= TouchPressed;
            touchActionPressed.Disable();
        }

        if (touchActionPosition != null)
        {
            touchActionPosition.Disable();
        }
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        if (touchActionPosition == null) return;

        Vector2 screenPosition = touchActionPosition.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);


        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue, 1f);

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log(hitObject.name);

            if (hitObject.CompareTag("Blueberry"))
            {
                if (audioSource != null) audioSource.Play();
                OnBlueberry.Invoke();
                Destroy(hitObject);
            }
        }
    }
}
