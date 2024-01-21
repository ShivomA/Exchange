using UnityEngine;

public class PlayerCombatInputAction : MonoBehaviour {
    public Animator playerAnimator;

    public GameObject attack1Projectile;
    public GameObject attack2Projectile;

    public Transform attack1ProjectileTransform;

    public string m1HA1Name = "Standing 1H Magic Attack 01";
    public string m2HA4Name = "Standing 2H Magic Attack 04";

    private PlayerInput playerInput;

    private bool m1HA1Pressed;
    private bool m2HA4Pressed;

    private void Start() {
        playerAnimator = GetComponent<Animator>();

        playerInput = new PlayerInput();

        playerInput.PlayerController.M1HA1.performed += ctx => m1HA1Pressed = ctx.ReadValueAsButton();
        playerInput.PlayerController.M2HA4.performed += ctx => m2HA4Pressed = ctx.ReadValueAsButton();

        playerInput.PlayerController.Enable();
    }

    private void Update() {
        if (m1HA1Pressed) {
            playerAnimator.Play(m1HA1Name);
            m1HA1Pressed = false;
        }
        if (m2HA4Pressed) {
            playerAnimator.Play(m2HA4Name);
            m2HA4Pressed = false;
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
