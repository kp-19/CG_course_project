using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public Camera playerCamera;
    public Camera shipCamera;
    public InputAction switchCameraAction;
    public InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        switchCameraAction = inputActions.Player.SwitchCamera;
    }
    private void OnEnable()
    {
        switchCameraAction.Enable();
        switchCameraAction.performed += ctx => SwitchCamera();
    }

    private void OnDisable()
    {
        switchCameraAction.performed -= ctx => SwitchCamera();
        switchCameraAction.Disable();
    }

    void Start()
    {
        ActivatePlayerCamera(); // Default to player cam at start
    }

    void SwitchCamera()
    {
        if (playerCamera.enabled)
        {
            ActivateShipCamera();
        }
        else
        {
            ActivatePlayerCamera();
        }
    }

    void ActivatePlayerCamera()
    {
        playerCamera.enabled = true;
        shipCamera.enabled = false;
        if (shipCamera.GetComponent<AudioListener>()) shipCamera.GetComponent<AudioListener>().enabled = false;
        if (playerCamera.GetComponent<AudioListener>()) playerCamera.GetComponent<AudioListener>().enabled = true;
    }

    void ActivateShipCamera()
    {
        playerCamera.enabled = false;
        shipCamera.enabled = true;
        if (playerCamera.GetComponent<AudioListener>()) playerCamera.GetComponent<AudioListener>().enabled = false;
        if (shipCamera.GetComponent<AudioListener>()) shipCamera.GetComponent<AudioListener>().enabled = true;
    }
}
