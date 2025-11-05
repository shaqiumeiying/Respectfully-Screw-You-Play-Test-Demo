using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Offset Settings")]
    public float height = 0f;         
    public float distance = 0f;        
    public float angle = 0f;          

    [Header("Follow Settings")]
    public float smoothSpeed = 5f;     

    private Vector3 refVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (!target) return;

        // calculate desired rotation based on angle
        Quaternion rotation = Quaternion.Euler(angle, 0f, 0f);

        // desired camera position relative to target
        Vector3 offsetDir = rotation * Vector3.back;
        Vector3 desiredPos = target.position + offsetDir * distance + Vector3.up * height;

        // smooth position transition
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref refVelocity, 1f / smoothSpeed);

        // look at player (can offset slightly forward)
        Vector3 lookTarget = target.position + Vector3.up * 1.5f;
        transform.LookAt(lookTarget);
    }
}
