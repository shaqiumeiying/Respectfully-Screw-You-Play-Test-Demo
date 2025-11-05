using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 initialScale;

    void Start()
    {
        mainCam = Camera.main;
        initialScale = transform.localScale;
    }

    void LateUpdate()
    {
        if (!mainCam) return;

        // Always face camera
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);

        // Keep constant size on screen
        float distance = Vector3.Distance(transform.position, mainCam.transform.position);
        float scaleFactor = distance * 0.1f;  // tweak multiplier if too big/small
        transform.localScale = initialScale / scaleFactor;
    }
}
