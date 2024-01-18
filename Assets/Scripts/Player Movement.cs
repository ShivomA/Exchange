using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float maxRunSpeed = 2;
    public float acceleration = 1;
    public float deceleration = 1;
    public float rapidDeceleration = 4;
    public float maxWalkingSpeed = 0.5f;
    public float thresholdVelocity = 0.5f;
    public float minThresholdVelocity = 0.01f;

    public Animator playerAnimator;

    [SerializeField]
    private float velocityX;
    [SerializeField]
    private float velocityZ;

    private int jumpHash;
    private int velocityXHash;
    private int velocityZHash;

    private float runInput;
    private bool jumpInput;
    private float xAxisInput;
    private float zAxisInput;

    private void Start() {
        playerAnimator = GetComponent<Animator>();
        jumpHash = Animator.StringToHash("Jump");
        velocityXHash = Animator.StringToHash("Velocity X");
        velocityZHash = Animator.StringToHash("Velocity Z");
    }

    private void Update() {
        jumpInput = Input.GetKeyDown(KeyCode.Space);
        xAxisInput = Input.GetAxis("Horizontal");
        zAxisInput = Input.GetAxis("Vertical");

        velocityX += xAxisInput * acceleration * Time.deltaTime;
        velocityZ += zAxisInput * acceleration * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            runInput = maxRunSpeed;
        } else {
            runInput = maxWalkingSpeed;
        }

        Decelerate();

        playerAnimator.SetFloat(velocityXHash, velocityX);
        playerAnimator.SetFloat(velocityZHash, velocityZ);

        if (jumpInput) {
            playerAnimator.SetTrigger(jumpHash);
        }
    }

    private void Decelerate() {
        if (xAxisInput == 0) {
            if (Mathf.Abs(velocityX) < minThresholdVelocity) {
                velocityX = 0;
            } else if (Mathf.Abs(velocityX) < thresholdVelocity) {
                velocityX -= velocityX * rapidDeceleration * Time.deltaTime;
            } else {
                velocityX -= velocityX * deceleration * Time.deltaTime;
            }
        }
        if (zAxisInput == 0) {
            if (Mathf.Abs(velocityZ) < minThresholdVelocity) {
                velocityZ = 0;
            } else if (Mathf.Abs(velocityZ) < thresholdVelocity) {
                velocityZ -= velocityZ * rapidDeceleration * Time.deltaTime;
            } else {
                velocityZ -= velocityZ * deceleration * Time.deltaTime;
            }
        }

        if (Mathf.Abs(velocityX) - runInput > thresholdVelocity) {
            velocityX -= velocityX * deceleration * Time.deltaTime;
        } else {
            velocityX = Mathf.Clamp(velocityX, -runInput, runInput);
        }


        if (Mathf.Abs(velocityZ) - runInput > thresholdVelocity) {
            velocityZ -= velocityZ * deceleration * Time.deltaTime;
        } else {
            velocityZ = Mathf.Clamp(velocityZ, -runInput, runInput);
        }
    }
}
