using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(AudioSource))]
public class EnemyReactionC : MonoBehaviour
{
    [Header("Reaction Settings")]
    public Color hitColor = Color.red;     // color shown when hit
    public float flashDuration = 0.1f;     // how long to stay flashed
    public AudioClip hitSound;             // optional hit sound

    private Renderer rend;
    private Color originalColor;
    private AudioSource audioSource;
    private bool flashing;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void ReactToHit()
    {
        if (!flashing)
            StartCoroutine(FlashColor());

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
}
