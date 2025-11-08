//using UnityEngine;

//[RequireComponent(typeof(Renderer))]
//[RequireComponent(typeof(AudioSource))]
//public class EnemyReactionC : MonoBehaviour
//{
//    [Header("Reaction Settings")]
//    public Color hitColor = Color.red;     // color shown when hit
//    public float flashDuration = 0.1f;     // how long to stay flashed
//    public AudioClip hitSound;             // optional hit sound

//    private Renderer rend;
//    private Color originalColor;
//    private AudioSource audioSource;
//    private bool flashing;

//    void Start()
//    {
//        rend = GetComponent<Renderer>();
//        originalColor = rend.material.color;
//        audioSource = GetComponent<AudioSource>();
//        audioSource.playOnAwake = false;
//    }

//    public void ReactToHit()
//    {
//        if (!flashing)
//            StartCoroutine(FlashColor());

//        if (hitSound && audioSource)
//            audioSource.PlayOneShot(hitSound);
//    }

//    private System.Collections.IEnumerator FlashColor()
//    {
//        flashing = true;
//        rend.material.color = hitColor;
//        yield return new WaitForSeconds(flashDuration);
//        rend.material.color = originalColor;
//        flashing = false;
//    }
//}
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(AudioSource))]
public class EnemyReactionC : MonoBehaviour
{
    [Header("Reaction Settings")]
    public int flashCount = 6;              // how many times to blink
    public float flashInterval = 0.15f;     // time between toggles
    public float transparentAlpha = 0.4f;   // how transparent during flash

    [Header("Audio & Feedback")]
    public AudioClip hitSound;
    public bool shakeCamera = true;
    public float shakeDuration = 0.15f;

    private Renderer rend;
    private Material mat;
    private Color originalColor;
    private AudioSource audioSource;
    private bool flashing = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material; // creates instance for this object
        originalColor = mat.color;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Ensure the material supports transparency
        if (mat.color.a == 1f)
        {
            Color c = mat.color;
            c.a = 1f;
            mat.color = c;
        }
    }

    public void ReactToHit()
    {
        if (!flashing)
            StartCoroutine(FlashOpacity());

        if (hitSound && audioSource)
            audioSource.PlayOneShot(hitSound);
    }

    private IEnumerator FlashOpacity()
    {
        flashing = true;

        // camera shake
        if (shakeCamera)
        {
            DramaticCamera camShake = FindObjectOfType<DramaticCamera>();
            if (camShake)
                camShake.TriggerShake(shakeDuration);
        }

        for (int i = 0; i < flashCount; i++)
        {
            // fade out (make transparent)
            Color c = originalColor;
            c.a = transparentAlpha;
            mat.color = c;
            yield return new WaitForSeconds(flashInterval);

            // fade back in (opaque)
            mat.color = originalColor;
            yield return new WaitForSeconds(flashInterval);
        }

        flashing = false;
    }
}
