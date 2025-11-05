using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackA : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 0.2f;   // attacks per 0.2s while held
    public float attackRange = 5f;
    public LayerMask enemyLayer;

    [Header("References")]
    public Transform aimOrigin;
    public RectTransform crosshairUI;
    public Image crosshairImage;          // assign your crosshair Image component here
    public Sprite defaultSprite;          // normal crosshair sprite
    public Sprite fingerSprite;           // sprite shown during attack
    public AudioClip attackSFX;           // short sound effect
    private AudioSource audioSource;      

    private Camera mainCam;
    private float lastAttackTime;
    private bool isAttacking;

    void Start()
    {
        mainCam = Camera.main;

        // Optional safety check
        if (!audioSource && attackSFX)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Read trigger axis (Fire1) and mouse/keyboard button (Fire2)
        float triggerValue = Input.GetAxis("Fire1");  // analog 0¨C1 for RT / R2
        bool triggerHeld = triggerValue > 0.5f;

        bool attackHeld = triggerHeld || Input.GetButton("Fire2"); // right mouse

        // Continuous attack while held
        if (attackHeld && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            PerformAttack();
        }

        // restore crosshair sprite if attack animation time has passed
        if (isAttacking && Time.time >= lastAttackTime + 0.25f)
        {
            crosshairImage.sprite = defaultSprite;
            isAttacking = false;
        }
    }

    void PerformAttack()
    {
        if (!mainCam || !crosshairUI) return;

        // Visual + sound feedback
        if (crosshairImage && fingerSprite)
        {
            crosshairImage.sprite = fingerSprite;
            isAttacking = true;
        }
        if (attackSFX && audioSource)
        {
            audioSource.PlayOneShot(attackSFX);
        }

        // Raycast from camera through crosshair
        Ray ray = mainCam.ScreenPointToRay(crosshairUI.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, enemyLayer))
        {
            Vector3 hitDir = (hit.point - aimOrigin.position).normalized;

            EnemyReactionA reaction = hit.collider.GetComponent<EnemyReactionA>();
            if (reaction)
                reaction.ReactToHit(hitDir);
        }
    }
}
