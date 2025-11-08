using UnityEngine;

public class DramaticCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Position Settings")]
    public Vector3 offset = new Vector3(0f, 8f, -6f);
    public float followSpeed = 10f;
    public float lookAheadMultiplier = 1.5f;

    [Header("Rotation Settings")]
    public float tiltAmount = 5f;
    public float rotationSmooth = 6f;

    [Header("Shake Settings")]
    public float shakeIntensity = 0.2f;
    public float shakeDecay = 1.5f;

    private Vector3 lastTargetPos;
    private Vector3 shakeOffset = Vector3.zero;
    private float shakeTimer = 0f;

    void Start()
    {
        if (target)
            lastTargetPos = target.position;
    }

    void LateUpdate()
    {
        if (!target) return;

        // --- follow ---
        Vector3 moveDelta = target.position - lastTargetPos;
        lastTargetPos = target.position;

        Vector3 leadOffset = moveDelta * lookAheadMultiplier;
        Vector3 desiredPos = target.position + offset + leadOffset;

        // Apply smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        // --- rotation / tilt ---
        float tiltX = -moveDelta.z * tiltAmount;
        float tiltZ = moveDelta.x * tiltAmount;

        Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position + Vector3.up * 1.5f);
        Quaternion tiltRot = lookRot * Quaternion.Euler(tiltX, 0f, tiltZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, tiltRot, Time.deltaTime * rotationSmooth);

        // --- apply camera shake ---
        if (shakeTimer > 0f)
        {
            shakeOffset = Random.insideUnitSphere * shakeIntensity * (shakeTimer);
            transform.position += shakeOffset;
            shakeTimer -= Time.deltaTime * shakeDecay;
        }
        else
        {
            shakeOffset = Vector3.zero;
        }
    }

    public void TriggerShake(float duration)
    {
        shakeTimer = duration;
    }
}
