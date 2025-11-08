using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (!mainCam) return;

        // Get direction from the texture to camera
        Vector3 dirToCam = mainCam.transform.position - transform.position;

        // Face the camera directly
        transform.rotation = Quaternion.LookRotation(-dirToCam);
    }
}
