using UnityEngine;
using UnityEngine.UI;

public class PlayerAim : MonoBehaviour
{
    [Header("Crosshair (UI)")]
    public RectTransform crosshairUI;
    public float aimRange = 300f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 10f;
    public float smoothFactor = 10f;
    public float inputThreshold = 0.1f;   // minimum input strength to count as "active"

    private Camera mainCam;
    private Vector3 lastScreenPos;
    private Vector2 currentOffset;
    private Vector3 lastLookDir = Vector3.forward;
    private bool usingController = false; // track which device is currently active

    void Start()
    {
        mainCam = Camera.main;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        lastScreenPos = mainCam.WorldToScreenPoint(transform.position);
    }

    void LateUpdate()
    {
        if (!crosshairUI) return;

        // Smooth screen position to avoid jump jitter
        Vector3 targetScreenPos = mainCam.WorldToScreenPoint(transform.position);
        Vector3 smoothedScreenPos = Vector3.Lerp(lastScreenPos, targetScreenPos, Time.deltaTime * smoothFactor);
        lastScreenPos = smoothedScreenPos;

        // Controller input
        float aimX = Input.GetAxis("RightStickHorizontal");
        float aimY = -Input.GetAxis("RightStickVertical"); // invert Y
        Vector2 stickInput = new Vector2(aimX, aimY);

        // Detect active input
        bool controllerActive = stickInput.sqrMagnitude > (inputThreshold * inputThreshold);
        bool mouseActive = Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;

        if (controllerActive)
        {
            usingController = true;
            currentOffset = stickInput.normalized * aimRange;
        }
        else if (mouseActive)
        {
            usingController = false;
            Vector2 mouseOffset = (Vector2)Input.mousePosition - (Vector2)smoothedScreenPos;
            if (mouseOffset.magnitude > aimRange)
                mouseOffset = mouseOffset.normalized * aimRange;
            currentOffset = mouseOffset;
        }
        // If neither active, keep last offset (no snapping)

        // Move crosshair
        crosshairUI.position = smoothedScreenPos + (Vector3)currentOffset;

        // Camera-relative rotation
        Vector3 camForward = mainCam.transform.forward;
        Vector3 camRight = mainCam.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 lookDir = (camRight * currentOffset.x + camForward * currentOffset.y);
        if (lookDir.sqrMagnitude > 0.001f)
            lastLookDir = lookDir.normalized;

        Quaternion targetRot = Quaternion.LookRotation(lastLookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
    }
}
