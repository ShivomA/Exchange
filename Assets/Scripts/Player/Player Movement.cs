using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    //[HideInInspector]
    public bool isCastingSpell;

    public float maxSpeed = 2;
    public float maxRunSpeed = 4;
    public float maxWalkSpeed = 2;
    public float acceleration = 10;
    public float deceleration = 10;
    public float rotationSpeed = 10;
    public float rapidDeceleration = 15;
    public float thresholdVelocity = 0.5f;
    public float minMovementVelocity = 0.1f;

    public Transform mainCamera;
    public Animator playerAnimator;

    private int velocityXHash;
    private int velocityZHash;

    private PlayerInput playerInput;
    private Vector2 movementInput;
    private bool isMovementPressed;
    private bool isJumpPressed;
    private bool isRunPressed;

    private Rigidbody rb;

    private void Start() {
        if (mainCamera == null) {
            mainCamera = Camera.main.transform;
        }

        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        velocityXHash = Animator.StringToHash("Velocity X");
        velocityZHash = Animator.StringToHash("Velocity Z");

        EnablePlayerInput();
    }

    private void EnablePlayerInput() {
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
        HandleRotation();
        HandleJump();

        if (Input.GetKeyDown(KeyCode.F)) { EnablePlayerInput(); }
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleJump() {
        if (isJumpPressed) {
            playerAnimator.Play("Fast Jumping");
            isJumpPressed = false;
        }
    }

    private void HandleMovement() {
        if (isCastingSpell) {
            rb.velocity = new(0, rb.velocity.y, 0);
        } else {
            Vector3 cameraSideDirection = new Vector3(mainCamera.right.x, 0, mainCamera.right.z).normalized;
            Vector3 cameraForwardDirection = new Vector3(mainCamera.forward.x, 0, mainCamera.forward.z).normalized;


            if (isMovementPressed) {
                rb.AddForce(movementInput.x * acceleration * cameraSideDirection);
                rb.AddForce(movementInput.y * acceleration * cameraForwardDirection);
            }

            if (isRunPressed) {
                maxSpeed = maxRunSpeed;
            } else {
                maxSpeed = maxWalkSpeed;
            }

            Decelerate();
        }

        Vector3 sideDirection = transform.right;
        Vector3 forwardDirection = transform.forward;

        float sideVelocity = Vector3.Dot(rb.velocity, sideDirection);
        float forwardVelocity = Vector3.Dot(rb.velocity, forwardDirection);

        playerAnimator.SetFloat(velocityXHash, sideVelocity);
        playerAnimator.SetFloat(velocityZHash, forwardVelocity);
    }

    private void HandleRotation() {
        if (!isMovementPressed) { return; }

        Vector3 cameraForwardDirection = new Vector3(mainCamera.forward.x, 0, mainCamera.forward.z).normalized;

        Quaternion currentRotation = transform.rotation;

        Quaternion targetRotation = Quaternion.LookRotation(cameraForwardDirection);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Decelerate() {
        Vector3 decelarationForce = Vector3.zero;
        Vector3 sideDirection = transform.right;
        Vector3 forwardDirection = transform.forward;

        if (!isMovementPressed || movementInput.x == 0) {
            if (Mathf.Abs(Vector3.Dot(rb.velocity, sideDirection)) < minMovementVelocity) {
                Vector3 forwardVelocity = Vector3.Dot(rb.velocity, transform.forward) * transform.forward;
                Vector3 upVelocity = Vector3.Dot(rb.velocity, transform.up) * transform.up;

                rb.velocity = forwardVelocity + upVelocity;
            } else if (Mathf.Abs(rb.velocity.x) < thresholdVelocity) {
                decelarationForce += Vector3.Dot(rb.velocity, sideDirection) * rapidDeceleration * sideDirection;
            } else {
                decelarationForce += Vector3.Dot(rb.velocity, sideDirection) * deceleration * sideDirection;
            }
        }
        if (!isMovementPressed || movementInput.y == 0) {
            if (Mathf.Abs(Vector3.Dot(rb.velocity, forwardDirection)) < minMovementVelocity) {
                Vector3 sideVelocity = Vector3.Dot(rb.velocity, transform.right) * transform.right;
                Vector3 upVelocity = Vector3.Dot(rb.velocity, transform.up) * transform.up;

                rb.velocity = sideVelocity + upVelocity;
            } else if (Mathf.Abs(rb.velocity.z) < thresholdVelocity) {
                decelarationForce += Vector3.Dot(rb.velocity, forwardDirection) * rapidDeceleration * forwardDirection;
            } else {
                decelarationForce += Vector3.Dot(rb.velocity, forwardDirection) * deceleration * forwardDirection;
            }
        }

        rb.AddForce(-decelarationForce);

        Vector2 movementVelocity = new(rb.velocity.x, rb.velocity.z);
        if (movementVelocity.magnitude - maxSpeed > thresholdVelocity) {
            rb.AddForce(rapidDeceleration * -new Vector3(movementVelocity.x, 0, movementVelocity.y));
        } else if (movementVelocity.magnitude > maxSpeed) {
            movementVelocity = movementVelocity.normalized * maxSpeed;
            rb.velocity = new(movementVelocity.x, rb.velocity.y, movementVelocity.y);
        }
    }
}
