using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class PlayerAnimDriver : MonoBehaviour
{
    // Small velocities under this count as idle (m/s)
    [Tooltip("Small velocities under this count as idle (m/s).")]
    [SerializeField] float idleCutoff = 0.05f;

    // Walk speed cap when not sprinting (m/s)
    [Tooltip("Walk speed in m/s (used to cap speed when not sprinting).")]
    [SerializeField] float walkMax = 2.9f;

    Animator anim;
    CharacterController cc;
    public ObiRopeClimber obiRopeClimber;
    bool crouch;

    void Awake()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {

        if (obiRopeClimber.climbing == true)
        {
            anim.SetBool("Climb", true);
        }
        else
        {
            anim.SetBool("Climb", false);
        }
        // Toggle crouch on C key press
        if (Input.GetKeyDown(KeyCode.C))
        {
            crouch = !crouch;
            anim.SetBool("Crouch", crouch);
        }
    }

    void LateUpdate()   // run after movement has updated
    {
        Vector3 v = cc.velocity;
        v.y = 0;                           // ignore vertical
        float speed = v.magnitude;         // ground speed

        // Treat tiny drift as zero
        if (speed < idleCutoff)
            speed = 0f;

        // Cap speed at walkMax unless sprinting
        if (!Input.GetKey(KeyCode.LeftShift))
            speed = Mathf.Min(speed, walkMax);

        anim.SetFloat("Speed", speed);
    }
}