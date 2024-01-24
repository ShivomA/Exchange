using UnityEngine;
using Cinemachine;

public class AimController : MonoBehaviour {
    public CinemachineFreeLook freeLookCamera;
    public CinemachineVirtualCamera aimingCamera;

    public float yFreeLookMaxSpeed = 20;
    public float xFreeLookMaxSpeed = 300;

    private PlayerInput playerInput;
    public bool isAiming;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        EnablePlayerInput();
        SetCameraSensitivity();
    }

    private void EnablePlayerInput() {
        playerInput = new PlayerInput();

        playerInput.PlayerController.Aim.performed += ctx => isAiming = ctx.ReadValueAsButton();

        playerInput.PlayerController.Enable();
    }

    private void SetCameraSensitivity() {
        freeLookCamera.m_YAxis.m_MaxSpeed = yFreeLookMaxSpeed;
        freeLookCamera.m_XAxis.m_MaxSpeed = xFreeLookMaxSpeed;
    }

    private void Update() {
        if (isAiming) {
            aimingCamera.gameObject.SetActive(true);
        } else {
            aimingCamera.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.F)) { EnablePlayerInput(); }
    }
}
