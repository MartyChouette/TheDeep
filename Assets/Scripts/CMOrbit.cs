using UnityEngine;

public class CMOrbit : MonoBehaviour
{
    [Header("Hold RMB to orbit")]
    public bool clickToOrbit = true;
    public float sensitivity = 4f;           // tweak
    public Vector2 pitchClamp = new(-45, 40); // up/down limits

    float yaw;   // left-right
    float pitch; // up-down


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    void Update()
    {
        bool wantOrbit = !clickToOrbit || Input.GetButton("Fire2"); // RMB
        if (!wantOrbit) return;

        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchClamp.x, pitchClamp.y);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}