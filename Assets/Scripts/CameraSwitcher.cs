using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public Camera playerCamera;
    public Camera shipCamera;
    public Transform playerTransform;
    public Transform shipTransform;
    public float switchDistanceThreshold = 3f;  // Must be close to switch

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
        ActivatePlayerCamera(); // Start with player camera
    }

    void SwitchCamera()
    {
        float distance = Vector3.Distance(playerTransform.position, shipTransform.position);

        if (distance <= switchDistanceThreshold)
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
        else
        {
            Debug.Log("Too far from the ship to switch camera!");
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
