using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour {
    public Animator playerAnimator;

    public GameObject attack1Projectile;
    public GameObject attack2Projectile;

    public Transform attack1ProjectileTransform;

    public string lastAttackAnimation;
    public string m1HA1Name = "Standing 1H Magic Attack 01";
    public string m2HA4Name = "Standing 2H Magic Attack 04";

    private PlayerMovement playerMovement;

    private PlayerInput playerInput;
    private bool m1HA1Pressed;
    private bool m2HA4Pressed;

    private void Start() {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        EnablePlayerInput();
    }

    private void EnablePlayerInput() {
        playerInput = new PlayerInput();

        playerInput.PlayerController.M1HA1.performed += ctx => m1HA1Pressed = ctx.ReadValueAsButton();
        playerInput.PlayerController.M2HA4.performed += ctx => m2HA4Pressed = ctx.ReadValueAsButton();

        playerInput.PlayerController.Enable();
    }

    private void Update() {
        if (m1HA1Pressed) {
            PlayAnimation(m1HA1Name);
        }
        if (m2HA4Pressed) {
            PlayAnimation(m2HA4Name);
        }

        CheckCurrentAnimation();

        if (Input.GetKeyDown(KeyCode.F)) { EnablePlayerInput(); }
    }

    private void PlayAnimation(string animationName) {
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Movement")) { return; }

        playerAnimator.Play(animationName);

        lastAttackAnimation = animationName;
        playerMovement.isCastingSpell = true;
    }

    private void CheckCurrentAnimation() {
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(lastAttackAnimation)) {
            playerMovement.isCastingSpell = true;
        } else {
            playerMovement.isCastingSpell = false;
        }
    }

    public void InstantiateProjectile(int inx) {
        if (inx == 1) {
            Instantiate(attack1Projectile, attack1ProjectileTransform.position, attack1ProjectileTransform.rotation);
        }
    }

    public void DisplayBeam(int active) {
        attack2Projectile.SetActive(active == 1);
    }
}
