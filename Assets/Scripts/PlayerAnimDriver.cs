using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimDriver : MonoBehaviour
{
    public float walkSpeed = 2f;     // tweak to match animation speed
    public float runSpeed = 4f;

    Animator anim;
    CharacterController cc;          // or use your own movement script

    void Awake()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // ?? movement magnitude ???????????????????????????????
        Vector3 vel = new Vector3(cc.velocity.x, 0, cc.velocity.z);
        float speed = vel.magnitude;

        // Scale into 0?1 (walk) and >1 (run).  Assumes
        //   walkSpeed == average magnitude while walking,
        //   runSpeed  == average magnitude while running.
        float norm = 0;
        if (speed > 0.01f)           // ignore micro jitter
            norm = Mathf.InverseLerp(0, walkSpeed, speed); // 0 -1 for walk
        if (Input.GetKey(KeyCode.LeftShift))
            norm = Mathf.InverseLerp(walkSpeed, runSpeed, speed) + 1f;

        anim.SetFloat("Speed", norm);

        // ?? crouch ???????????????????????????????????????????
        anim.SetBool("Crouch", Input.GetKey(KeyCode.C));

        // ?? rope climb (placeholder) ?????????????????????????
        // anim.SetBool("Climb", onRope);  // set this when you implement ropes
    }
}
