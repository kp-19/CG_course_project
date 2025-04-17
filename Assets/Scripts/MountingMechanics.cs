using UnityEngine;
using UnityEngine.InputSystem;

public class MountingMechanics : MonoBehaviour
{
    public Transform ship; // Reference to the ship GameObject
    public Transform mountPoint; // Where the player should be placed when mounting the ship

    private bool isMounted = false;
    private Vector3 dismountOffset;

    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Mount.performed += OnMountPressed;
    }

    void OnDisable()
    {
        inputActions.Player.Mount.performed -= OnMountPressed;
        inputActions.Disable();
    }

    private void OnMountPressed(InputAction.CallbackContext context)
    {
        if (!isMounted)
        {
            TryMountShip();
        }
        else
        {
            DismountShip();
        }
    }

    void TryMountShip()
    {
        if (ship == null || mountPoint == null) return;

        // Calculate the relative offset before mounting
        dismountOffset = transform.position - ship.position;

        // Move player to mount point and parent to ship
        transform.position = mountPoint.position;
        transform.SetParent(ship);

        isMounted = true;
    }

    void DismountShip()
    {
        // Unparent the player and return to relative dismount position
        transform.SetParent(null);
        transform.position = ship.position + dismountOffset;

        isMounted = false;
    }
}
