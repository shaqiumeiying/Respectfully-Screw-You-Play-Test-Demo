using UnityEngine;

public class PlayerShootBasic : MonoBehaviour
{
    [Header("Aim Settings")]
    public Camera mainCam;              // assign your main camera
    public float rotationSpeed = 10f;   // how fast to turn
    public LayerMask groundLayer;       // layer to aim at (usually "Ground")

    [Header("Shooting")]
    public KeyCode shootKey = KeyCode.Mouse0; // left click to shoot

    private Vector3 aimPoint; // store where the player is aiming

    void Update()
    {
        RotateTowardMouse();

        // draw the debug line every frame
        Debug.DrawLine(transform.position + Vector3.up * 1f, aimPoint, Color.yellow);

        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
        }
    }

    void RotateTowardMouse()
    {
        if (!mainCam) return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, groundLayer))
        {
            aimPoint = hitInfo.point;

            Vector3 dir = (aimPoint - transform.position);
            dir.y = 0; // ignore vertical difference

            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void Shoot()
    {
        Debug.Log("Pew! Pew! The player just shot toward " + aimPoint);
    }
}
