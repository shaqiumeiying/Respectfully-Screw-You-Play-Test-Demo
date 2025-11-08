using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(AudioSource))]
public class EnemyReactionB : MonoBehaviour
{
    [Header("Reaction Settings")]
    public Color hitColor = Color.red;     // color shown when hit
    public float flashDuration = 0.1f;     // how long to stay flashed
    public AudioClip hitSound;             // optional hit sound

    [Header("Jitter Settings")]
    public float shakeDuration = 0.15f;    // total shake time
    public float shakeMagnitude = 0.08f;   // how far to move left/right

    private Renderer rend;
    private Color originalColor;
    private AudioSource audioSource;
    private bool flashing;
    private bool shaking;
    private Vector3 originalPos;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        originalPos = transform.localPosition;
    }

    public void ReactToHit()
    {
        if (!flashing)
            StartCoroutine(FlashColor());

        if (!shaking)
            StartCoroutine(Shake());

        if (hitSound && audioSource)
            audioSource.PlayOneShot(hitSound);
    }

    private System.Collections.IEnumerator FlashColor()
    {
        flashing = true;
        rend.material.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = originalColor;
        flashing = false;
    }

    private System.Collections.IEnumerator Shake()
    {
        shaking = true;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float xOffset = Mathf.Sin(elapsed * 80f) * shakeMagnitude;
            transform.localPosition = originalPos + new Vector3(xOffset, 0, 0);
            yield return null;
        }

        transform.localPosition = originalPos;
        shaking = false;
    }
}
