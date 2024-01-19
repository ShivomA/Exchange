using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    public Animator playerAnimator;

    public GameObject attack1Projectile;
    public GameObject attack2Projectile;

    public Transform attack1ProjectileTransform;

    public string m1HA1Name = "Standing 1H Magic Attack 01";
    public string m2HA4Name = "Standing 2H Magic Attack 04";

    private void Start() {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            playerAnimator.Play(m1HA1Name);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            playerAnimator.Play(m2HA4Name);
        }
    }

    public void InstantiateProjectile(int inx) {
        if(inx == 1) {
            Instantiate(attack1Projectile, attack1ProjectileTransform.position, attack1ProjectileTransform.rotation);
        }
    }

    public void DisplayBeam(int active) {
        attack2Projectile.SetActive(active == 1);
    }
}
