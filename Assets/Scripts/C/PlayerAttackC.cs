using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackC : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 0.25f;     // delay between shots

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;      // assign bullet prefab
    public Transform firePoint;              // empty child of player
    public float projectileSpeed = 25f;

    [Header("Audio")]
    public AudioClip shootSFX;
    private AudioSource audioSource;

    private Camera mainCam;
    private DramaticCamera camShake;          // reference to DramaticCamera
    private float lastAttackTime;
    private bool isAttacking;

    void Start()
    {
        mainCam = Camera.main;
        if (!mainCam)
            Debug.LogError("No MainCamera found in scene. Make sure it is tagged 'MainCamera'.");

        // get the DramaticCamera component from MainCamera
        camShake = mainCam.GetComponent<DramaticCamera>();
        if (!camShake)
            Debug.LogWarning("No DramaticCamera component found on MainCamera.");

        // ensure audio source exists
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        // input detection
        float triggerValue = Input.GetAxis("Fire1");
        bool triggerHeld = triggerValue > 0.5f;
        bool attackHeld = triggerHeld || Input.GetButton("Fire2");

        // Fire when held, respecting cooldown
        if (attackHeld && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            ShootProjectile();
        }

        // restore crosshair sprite after short delay
        if (isAttacking && Time.time >= lastAttackTime + 0.25f)
        {
            isAttacking = false;
        }
    }

    void ShootProjectile()
    {
        if (!firePoint || !projectilePrefab) return;

        if (shootSFX && audioSource)
            audioSource.PlayOneShot(shootSFX);

        // --- determine shooting direction ---
        Vector3 shootDir = firePoint.forward;

        // --- spawn projectile ---
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(shootDir));
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        if (rb)
        {
            rb.velocity = shootDir * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("Projectile prefab missing Rigidbody.");
        }

        // --- camera shake ---
        if (camShake)
            camShake.TriggerShake(0.2f);

        // --- auto destroy projectile ---
        Destroy(proj, 1f);
    }
}
