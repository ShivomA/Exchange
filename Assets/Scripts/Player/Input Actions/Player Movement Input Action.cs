using UnityEngine;

public class PlayerMovementInputAction : MonoBehaviour {
    public Animator playerAnimator;

    private int isWalkingHash;
    private int isRunningHash;

    PlayerInput playerInput;
    Vector2 movementInput;
    bool isMovementPressed;
    bool isJumpPressed;
    bool isRunPressed;

    private void Start() {
        playerAnimator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("Is Walking");
        isRunningHash = Animator.StringToHash("Is Running");

        playerInput = new PlayerInput();

        playerInput.PlayerController.Movements.performed += ctx => {
            movementInput = ctx.ReadValue<Vector2>();
            isMovementPressed = movementInput.x != 0 || movementInput.y != 0;
        };

        playerInput.PlayerController.Run.performed += ctx => isRunPressed = ctx.ReadValueAsButton();

        playerInput.PlayerController.Jump.performed += ctx => isJumpPressed = ctx.ReadValueAsButton();

        playerInput.PlayerController.Enable();
    }

    private void Update() {
        HandleMovement();
        HandleRotation();
        HandleJump();
    }

    private void HandleJump() {
        if (isJumpPressed) {
            playerAnimator.Play("Jumping");
        }
    }

    private void HandleMovement() {
        bool isWalking = playerAnimator.GetBool(isWalkingHash);
        bool isRunning = playerAnimator.GetBool(isRunningHash);

        if (isMovementPressed) {
            if (!isWalking) {
                playerAnimator.SetBool(isWalkingHash, true);
            }
            if (isRunPressed && !isRunning) {
                playerAnimator.SetBool(isRunningHash, true);
            } else if (!isRunPressed && isRunning) {
                playerAnimator.SetBool(isRunningHash, false);
            }
        } else {
            if (isWalking) {
                playerAnimator.SetBool(isWalkingHash, false);
            }
            if (isRunning) {
                playerAnimator.SetBool(isRunningHash, false);
            }
        }
    }

    private void HandleRotation() {
        if (!isMovementPressed) { return; }

        Vector3 positionToLookAt = transform.position + new Vector3(movementInput.x, 0, movementInput.y);
        transform.LookAt(positionToLookAt);
    }
}
